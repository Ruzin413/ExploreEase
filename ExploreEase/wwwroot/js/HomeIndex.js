$(document).ready(function () {

    // -------------------- Recommendations --------------------
    function fetchRecommendedPackages() {
        $.ajax({
            url: '/Home/GetRecommendationsForCurrentUser',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                let html = '';
                if (Array.isArray(data) && data.length > 0) {
                    data.forEach(function (pkg) {
                        html += `
                            <div class="tour-card shadow" data-id="${pkg.tourPackageId}" style="cursor:pointer;">
                                <img src="${pkg.destinationImage}" class="card-img-top" alt="${pkg.name}">
                                <div class="card-body">
                                    <h5 class="card-title">${pkg.name}</h5>
                                    <h5 class="card-title">${pkg.numberOfDays} days</h5>
                                    <div class="description">${pkg.description.substring(0, 80)}...</div>
                                    <p class="card-text"><strong>Destination:</strong> ${pkg.destination}</p>
                                    <p class="card-text"><strong>Rating:</strong> ${pkg.rating}</p>
                                    <p class="card-text"><strong>Price:</strong> ₹${pkg.price}</p>
                                </div>
                            </div>`;
                    });
                } else {
                    html = '<div class="alert alert-info">No recommendations found.</div>';
                }
                $('#recPackageContainer').html(html);
                initScrollButtons('rec');
            },
            error: function () {
                $('#recPackageContainer').html('<div class="alert alert-danger">Could not load recommendations.</div>');
            }
        });
    }

    // -------------------- Tour Packages --------------------
    function fetchTourPackages() {
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
                                <h5 class="card-title">${pkg.numberOfDays} days</h5>
                                <div class="description">${pkg.description.substring(0, 80)}...</div>
                                <p class="card-text"><strong>Destination:</strong> ${pkg.destination}</p>
                                <p class="card-text"><strong>Rating:</strong> ${pkg.rating}</p>
                                <p class="card-text"><strong>Price:</strong> ₹${pkg.price}</p>
                            </div>
                        </div>`;
                });
                $('#pkgPackageContainer').html(html);
                initScrollButtons('pkg');
            },
            error: function () {
                $('#pkgPackageContainer').html('<div class="alert alert-danger">Could not load tour packages.</div>');
            }
        });
    }

    // -------------------- Scroll Button Initializer --------------------
    function initScrollButtons(prefix) {
        const container = document.getElementById(`${prefix}PackageContainer`);
        const scrollLeftBtn = document.getElementById(`${prefix}ScrollLeft`);
        const scrollRightBtn = document.getElementById(`${prefix}ScrollRight`);

        if (!container || !scrollLeftBtn || !scrollRightBtn) return;

        const cardWidth = 320;
        const scrollAmount = cardWidth * 3;

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
    
    // -------------------- Shared Click Event --------------------
    $(document).on('click', '.tour-card', function () {
        const id = $(this).data('id');
        window.location.href = `/UserActivity/User/Booking?id=${id}`;
    });
    // Initial load for both sections
    fetchRecommendedPackages();
    fetchTourPackages();
});
