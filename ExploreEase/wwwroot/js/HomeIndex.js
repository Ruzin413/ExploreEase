$(document).ready(function () {
    $.ajax({
        url: '/Home/TourPackage',
        method: 'GET',
        success: function (data) {
            let html = '';
            data.forEach(function (pkg) {
                html += `
                    <div class="tour-card shadow" data-id="${pkg.tourPackageId}" style="cursor:pointer;">
                        <img src="${pkg.destinationImage}" class="card-img-top" alt="${pkg.name}">
                        <div class="card-body">
                            <h5 class="card-title">${pkg.name}</h5>
                            <h5 class="card-title">${pkg.numberOfDays}</h5>
                            <div class="description">${pkg.description.substring(0, 80)}...</div>
                            <p class="card-text"><strong>Destination:</strong> ${pkg.destination}</p>
                            <p class="card-text"><strong>Rating:</strong> ${pkg.rating}</p>
                            <p class="card-text"><strong>Price:</strong> ₹${pkg.price}</p>
                        </div>
                    </div>`;
            });

            $('#packageContainer').html(html);
            initScrollButtons();
        },
        error: function () {
            $('#packageContainer').html('<div class="alert alert-danger">Could not load tour packages.</div>');
        }
    });

    // ✅ Use event delegation
    $(document).on('click', '.tour-card', function () {
        const id = $(this).data('id');
        console.log('Navigating to package ID:', id); // debug log
        window.location.href = `/UserActivity/User/Booking?id=${id}`;
    });

    function initScrollButtons() {
        const container = document.getElementById('packageContainer');
        const scrollLeftBtn = document.getElementById('scrollLeft');
        const scrollRightBtn = document.getElementById('scrollRight');

        if (!container || !scrollLeftBtn || !scrollRightBtn) return;

        const cardWidth = 320;
        const scrollAmount = cardWidth * 4;

        scrollLeftBtn.addEventListener('click', function () {
            container.scrollLeft -= scrollAmount;
            updateButtonState();
        });

        scrollRightBtn.addEventListener('click', function () {
            container.scrollLeft += scrollAmount;
            updateButtonState();
        });

        container.addEventListener('scroll', updateButtonState);

        function updateButtonState() {
            scrollLeftBtn.disabled = container.scrollLeft <= 0;
            const maxScrollLeft = container.scrollWidth - container.clientWidth;
            scrollRightBtn.disabled = Math.abs(container.scrollLeft - maxScrollLeft) < 10;
        }

        updateButtonState();
    }
});
