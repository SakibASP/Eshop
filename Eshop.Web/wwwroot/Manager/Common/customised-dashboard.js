//Chart.register(ChartDataLabels);
document.addEventListener("DOMContentLoaded", function () {
    // Call the function to fetch data and render the chart
    SocialMediaMaintenanceDataChart();
    KeywordsPositionDataChart();
    BacklinkDataChart();
})


const SocialMediaMaintenanceDataChart = async () => {
    try {
        const url = "/Dashboard/GetSocialMediaMaintenanceData";
        const response = await fetch(url);
        const tableData = await response.json(); // Assuming the response is JSON

        //console.log('Fetched data:', tableData);
        // Extract unique months and platforms
        const months = [...new Set(tableData.map(item => item.Months))];
        const platforms = [...new Set(tableData.map(item => item.Platforms))];

        //alert(months);
        //alert(platforms);

        // Prepare data for Chart.js
        const data = {
            labels: months,
            datasets: platforms.map(platform => ({
                label: platform,
                data: months.map(month => {
                    const postEntry = tableData.find(item => item.Months === month && item.Platforms === platform);
                    return postEntry ? postEntry.Posts : 0; // Default to 0 if no posts
                }),
                backgroundColor: `rgba(${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, 0.6)`,
            }))
        };

        // Calculate platform-wise totals
        const platformTotals = platforms.map((platform, index) => {
            return {
                platform: platform,
                total: data.datasets[index].data.reduce((acc, val) => acc + val, 0)  // Sum up all posts for the platform
            };
        });

        // Create the chart
        const ctx = document.getElementById('barChart');
        new Chart(ctx, {
            type: 'bar',
            data: data,
            options: {
                responsive: true,
                scales: {
                    x: {
                        stacked: false,
                    },
                    y: {
                        beginAtZero: true,
                    }
                },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: {
                            generateLabels: function (chart) {
                                return chart.data.datasets.map(function (dataset, index) {
                                    const platformTotal = platformTotals[index].total;
                                    return {
                                        text: `${dataset.label}: ${platformTotal}`,  // Show platform and its total posts
                                        fillStyle: dataset.backgroundColor,  // Use background color for each platform
                                        datasetIndex: index  // This enables toggling on legend click
                                    };
                                });
                            }
                        }
                    },
                    title: {
                        display: true,
                        text: 'Monthly Posts Comparison by Platform [Last 6 Months]'
                    }
                }
            }
        });
    } catch (error) {
        console.error('Error fetching data:', error);
    }
};


const KeywordsPositionDataChart = async () => {
    try {
        const url = "/Dashboard/GetKeywordsPositionData";
        const response = await fetch(url);
        const positions = await response.json(); // Assuming the response is JSON

        const chartData = positions[0]; // Access the first item in the array
        //console.log('Fetched data:', chartData);
        const _labels = ['Top Ten', 'Ten to Hundred', 'Above Hundred'];
        const _data = [chartData.TopTen, chartData.TenToHundred, chartData.AboveHundred];
        // Calculate the sum of all values
        const totalSum = _data.reduce((acc, val) => acc + val, 0);

        const ctx = document.getElementById('pieChart');
        new Chart(ctx, {
            type: 'pie',
            data: {
                datasets: [{
                    data: _data,
                    backgroundColor: [
                        "rgba(0, 150, 136, 0.76)",
                        "#30a44ae6",
                        "#290d69e6",
                        "#169398"
                    ],
                    hoverBackgroundColor: [
                        "rgba(0, 150, 136, 0.76)",
                        "rgba(0, 150, 136, 0.76)",
                        "rgba(0, 150, 136, 0.76)",
                        "rgba(0,0,0,0.07)"
                    ]
                }],
                labels: _labels
            },
            options: {
                responsive: true,
                tooltips: {
                    enabled: false
                },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: {
                            generateLabels: function (chart) {
                                // Create custom legend items showing values
                                return chart.data.labels.map(function (label, index) {
                                    const value = chart.data.datasets[0].data[index];
                                    return {
                                        text: `${label}: ${value}`,  // Show label and its value
                                        fillStyle: chart.data.datasets[0].backgroundColor[index],
                                        datasetIndex: 0,  // Reference the dataset
                                        index: index  // Reference the specific data index for toggling
                                    };
                                });
                            }
                        }
                    },
                    title: {
                        display: true,
                        text: `Total Keywords: ${totalSum} [Last 6 Months]` // Dynamically set the title with sum
                    }
                }
            }
        });

    } catch (error) {
        console.error('Error fetching data:', error);
    }
};

const BacklinkDataChart = async () => {
    try {
        const url = "/Dashboard/GetBacklinksData";
        const response = await fetch(url);
        const tableData = await response.json(); // Assuming the response is JSON

        //console.log('Fetched data:', tableData);
        // Extract unique months and platforms
        const months = [...new Set(tableData.map(item => item.Months))];
        const types = [...new Set(tableData.map(item => item.Types))];

        //alert(months);
        //alert(types);

        // Prepare data for Chart.js
        const data = {
            labels: months,
            datasets: types.map(type => ({
                label: type,
                data: months.map(month => {
                    const backlinks = tableData.find(item => item.Months === month && item.Types === type);
                    return backlinks ? backlinks.Backlinks : 0; // Default to 0 if no posts
                }),
                backgroundColor: `rgba(${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, 0.6)`,
                borderColor: `rgba(${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, 1)`, // Line color
                fill: false, // No fill under the line
                tension: 0.1, // Line smoothness
                //borderWidth: "1"
            }))
        };

        // Calculate type-wise totals
        const typeTotals = types.map((type, index) => {
            return {
                type: type,
                total: data.datasets[index].data.reduce((acc, val) => acc + val, 0)  // Sum up all posts for the platform
            };
        });

        // Create the chart
        const ctx = document.getElementById('lineChart');
        new Chart(ctx, {
            type: 'line',
            data: data,
            options: {
                responsive: true,
                scales: {
                    x: {
                        stacked: false,
                    },
                    y: {
                        beginAtZero: true,
                    }
                },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: {
                            generateLabels: function (chart) {
                                return chart.data.datasets.map(function (dataset, index) {
                                    const typeTotal = typeTotals[index].total;
                                    return {
                                        text: `${dataset.label}: ${typeTotal}`,  // Show types and its total backlinks
                                        fillStyle: dataset.backgroundColor,  // Use background color for each types
                                        datasetIndex: index  // This enables toggling on legend click
                                    };
                                });
                            }
                        },
                    },
                    title: {
                        display: true,
                        text: 'Monthly Backlinks Comparison by Types [Last 6 Months]'
                    }
                },
                animations: {
                    tension: {
                        duration: 1000,
                        easing: 'linear',
                        from: 1,
                        to: 0,
                        loop: true
                    }
                },
                scales: {
                    y: { // defining min and max so hiding the dataset does not change scale range
                        min: this.min,
                        max: this.max
                    }
                }
            }
        });
    } catch (error) {
        console.error('Error fetching data:', error);
    }
};
