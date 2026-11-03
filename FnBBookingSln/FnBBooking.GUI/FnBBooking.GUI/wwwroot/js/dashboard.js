window.setupDashboardCharts = (canvasId, label, dataValues, labels, chartType) => {
    const ctx = document.getElementById(canvasId).getContext('2d');

    // Hủy biểu đồ cũ nếu tồn tại để tránh lỗi đè dữ liệu khi render lại
    if (window[canvasId] instanceof Chart) {
        window[canvasId].destroy();
    }

    window[canvasId] = new Chart(ctx, {
        type: chartType, // 'line' hoặc 'bar' hoặc 'doughnut'
        data: {
            labels: labels,
            datasets: [{
                label: label,
                data: dataValues,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 2,
                fill: true,
                tension: 0.4 // Tạo độ cong cho đường biểu đồ
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
};