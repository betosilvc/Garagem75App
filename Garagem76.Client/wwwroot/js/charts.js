window.garagemCharts = {

criarBarChart: function(canvasId, labels, data, titulo) {

        const ctx = document.getElementById(canvasId);

        if (!ctx) return;

        // destrói se já existir (evita bug)
        if (ctx.chart)
        {
            ctx.chart.destroy();
        }

        ctx.chart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: titulo,
                    data: data
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        labels: {
                            color: "#fff"
                        }
                    }
                },
                scales: {
                    x: {
                        ticks: { color: "#ccc" }
                    },
                    y: {
                        ticks: { color: "#ccc" }
                    }
                }
            }
        });
    }

}
;