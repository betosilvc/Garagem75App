const CACHE_NAME = 'garagem75-v1';
const CACHE_STATIC = 'garagem75-static-v1';

// Arquivos essenciais que ficam em cache (shell do app)
const STATIC_ASSETS = [
    '/',
    '/manifest.json',
    '/img/icons/icon-192x192.png',
    '/img/icons/icon-512x512.png',
];

// ── INSTALL: faz cache dos assets estáticos ──────────────────────
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_STATIC).then(cache => {
            return cache.addAll(STATIC_ASSETS);
        })
    );
    // Ativa imediatamente sem esperar aba antiga fechar
    self.skipWaiting();
});

// ── ACTIVATE: limpa caches antigos ──────────────────────────────
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(keys =>
            Promise.all(
                keys
                    .filter(key => key !== CACHE_NAME && key !== CACHE_STATIC)
                    .map(key => caches.delete(key))
            )
        )
    );
    self.clients.claim();
});

// ── FETCH: estratégia por tipo de recurso ───────────────────────
self.addEventListener('fetch', event => {
    const { request } = event;
    const url = new URL(request.url);

    // Ignora requisições não-GET e de outras origens (ex: Google Maps)
    if (request.method !== 'GET') return;
    if (url.origin !== location.origin) return;

    // Formulários e rotas de API: sempre vai para a rede
    if (
        url.pathname.startsWith('/api/') ||
        url.pathname.startsWith('/Home/Enviar') ||
        request.headers.get('Accept')?.includes('application/json')
    ) {
        event.respondWith(networkOnly(request));
        return;
    }

    // Imagens: cache primeiro, busca na rede se não tiver
    if (request.destination === 'image') {
        event.respondWith(cacheFirst(request));
        return;
    }

    // Fontes e CSS/JS estáticos: cache primeiro
    if (
        url.pathname.startsWith('/css/') ||
        url.pathname.startsWith('/js/') ||
        url.pathname.startsWith('/lib/') ||
        request.destination === 'font'
    ) {
        event.respondWith(cacheFirst(request));
        return;
    }

    // Páginas HTML: rede primeiro, cache como fallback
    event.respondWith(networkFirst(request));
});

// ── ESTRATÉGIAS ─────────────────────────────────────────────────

// Network only — sem cache
async function networkOnly(request) {
    return fetch(request);
}

// Cache first — serve do cache, atualiza em background
async function cacheFirst(request) {
    const cached = await caches.match(request);
    if (cached) return cached;

    try {
        const response = await fetch(request);
        if (response.ok) {
            const cache = await caches.open(CACHE_NAME);
            cache.put(request, response.clone());
        }
        return response;
    } catch {
        // Sem cache e sem rede: retorna placeholder vazio para imagens
        if (request.destination === 'image') {
            return new Response(
                '<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200"><rect width="200" height="200" fill="#1a1a1a"/></svg>',
                { headers: { 'Content-Type': 'image/svg+xml' } }
            );
        }
        return new Response('Offline', { status: 503 });
    }
}

// Network first — tenta rede, usa cache se falhar
async function networkFirst(request) {
    try {
        const response = await fetch(request);
        if (response.ok) {
            const cache = await caches.open(CACHE_NAME);
            cache.put(request, response.clone());
        }
        return response;
    } catch {
        const cached = await caches.match(request);
        if (cached) return cached;

        // Fallback offline: página em cache raiz
        const fallback = await caches.match('/');
        return fallback || new Response('Você está offline.', { status: 503 });
    }
}