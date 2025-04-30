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

            // Initialize scroll buttons
            initScrollButtons();
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

    // Scroll functionality
    function initScrollButtons() {
        const container = document.getElementById('packageContainer');
        const scrollLeftBtn = document.getElementById('scrollLeft');
        const scrollRightBtn = document.getElementById('scrollRight');

        // Calculate scroll amount (approx width of one card including margin)
        const cardWidth = 320; // 300px width + 20px margin
        const scrollAmount = cardWidth * 4; // Scroll 4 cards at a time

        // Initial button state
        updateButtonState();

        // Scroll left
        scrollLeftBtn.addEventListener('click', function () {
            container.scrollLeft -= scrollAmount;
            updateButtonState();
        });

        // Scroll right
        scrollRightBtn.addEventListener('click', function () {
            container.scrollLeft += scrollAmount;
            updateButtonState();
        });

        // Update button state based on scroll position
        container.addEventListener('scroll', updateButtonState);

        function updateButtonState() {
            // Check if can scroll left
            scrollLeftBtn.disabled = container.scrollLeft <= 0;

            // Check if can scroll right
            const maxScrollLeft = container.scrollWidth - container.clientWidth;
            scrollRightBtn.disabled = Math.abs(container.scrollLeft - maxScrollLeft) < 10;
        }
    }
});