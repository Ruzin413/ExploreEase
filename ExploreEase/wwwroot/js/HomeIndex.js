$(document).ready(function () {
    $.ajax({
        url: '/Home/TourPackage',
        method: 'GET',
        success: function (data) {
            console.log("Data received from the server:", data);
            let html = '';
            data.forEach(function (pkg) {
                html += `
                            <div class="tour-card shadow">
                                <img src="${pkg.destinationImage}" class="card-img-top" alt="${pkg.name}">
                                <div class="card-body">
                                    <h5 class="card-title">${pkg.name}</h5>
                                    <div class="description">${pkg.description.substring(0, 80)}...</div>
                                    <p class="card-text"><strong>Destination:</strong> ${pkg.destination}</p>
                                    <p class="card-text"><strong>Rating:</strong> ${pkg.rating}</p>
                                    <p class="card-text"><strong>Price:</strong> ₹${pkg.price}</p>
                                </div>
                            </div>`;
            });
            $('#packageContainer').html(html);

            // Add explicit hover handlers for browsers that might need it
            $('.tour-card').hover(
                function () {
                    $(this).find('.description').css({
                        'height': 'auto',
                        'opacity': 1,
                        'margin-bottom': '0.5rem'
                    });
                },
                function () {
                    $(this).find('.description').css({
                        'height': 0,
                        'opacity': 0,
                        'margin-bottom': 0
                    });
                }
            );
        },
        error: function () {
            $('#packageContainer').html('<div class="alert alert-danger">Could not load tour packages.</div>');
        }
    });

    // Update clock
    function updateClock() {
        const now = new Date();
        $('#clock').text(now.toLocaleTimeString());
    }

    updateClock();
    setInterval(updateClock, 1000);
});