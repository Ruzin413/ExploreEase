$(document).ready(function () {
    function fetchAllPackages() {
        $.ajax({
            url: '/Admin/Admin/AllPackages',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                displayPackages(data);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching packages:", error);
                alert("Failed to load tour packages. Please try again later.");
            }
        });
    }

    function displayPackages(packages) {
        const packageGrid = $('#packageGrid');
        packageGrid.empty();

        packages.forEach(function (pkg) {
            const stars = getStarRating(pkg.rating);

            const card = `
                        <div class="col">
                            <div class="card">
                                <img src="${pkg.destinationImage}" class="card-img-top" alt="${pkg.destination}">
                                <div class="card-body">
                                    <h5 class="card-title">${pkg.name}</h5>
                                    <p class="card-text">
                                        <strong>Destination:</strong> ${pkg.destination}<br>
                                        <strong>Duration:</strong> ${pkg.numberOfDays} days<br>
                                        <strong>Price:</strong> Rs${pkg.price}<br>
                                        <strong>Rating:</strong> <span class="star-rating">${stars}</span>
                                    </p>
                                    <div class="d-flex justify-content-between mt-3">
                                        <button class="btn btn-update update-btn" data-id="${pkg.tourPackageId}" data-price="${pkg.price}">
                                            <i class="fas fa-edit"></i> Update
                                        </button>
                                        <button class="btn btn-delete delete-btn" data-id="${pkg.tourPackageId}">
                                            <i class="fas fa-trash"></i> Delete
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;

            packageGrid.append(card);
        });
    }

    function getStarRating(rating) {
        let stars = '';
        let fullStars = Math.floor(rating);
        let halfStar = rating % 1 >= 0.5;

        for (let i = 0; i < fullStars; i++) {
            stars += '<i class="fas fa-star"></i> ';
        }

        if (halfStar) {
            stars += '<i class="fas fa-star-half-alt"></i> ';
        }

        const emptyStars = 5 - fullStars - (halfStar ? 1 : 0);
        for (let i = 0; i < emptyStars; i++) {
            stars += '<i class="far fa-star"></i> ';
        }

        return stars;
    }

    // Open update modal
    $(document).on('click', '.update-btn', function () {
        const packageId = $(this).data('id');
        const currentPrice = $(this).data('price');

        $('#tourPackageId').val(packageId);
        $('#updatedPrice').val(currentPrice);
        $('#updateConfirmBtn').html('Yes, Update').prop('disabled', false);
        $('#updatePriceModal').modal('show');
    });

    // Handle update form
    $('#updatePriceForm').on('submit', function (e) {
        e.preventDefault();

        const $btn = $('#updateConfirmBtn');
        const packageId = $('#tourPackageId').val();
        const updatedPrice = $('#updatedPrice').val();

        $btn.html(`<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Updating...`);
        $btn.prop('disabled', true);

        $.ajax({
            url: '/Admin/Admin/UpdatePackagePrice',
            method: 'POST',
            data: {
                tourPackageId: packageId,
                updatedPrice: updatedPrice
            },
            success: function () {
                $btn.html(`<i class="fas fa-check"></i> Updated`);
                setTimeout(() => {
                    $('#updatePriceModal').modal('hide');
                    fetchAllPackages();
                }, 1000);
            },
            error: function () {
                $btn.html(`<i class="fas fa-exclamation-triangle"></i> Error`);
                setTimeout(() => {
                    $btn.html('Yes, Update').prop('disabled', false);
                }, 2000);
            }
        });
    });

    // Open delete modal
    $(document).on('click', '.delete-btn', function () {
        const packageId = $(this).data('id');
        $('#confirmDelete').data('id', packageId).html('Delete').prop('disabled', false);
        $('#deleteConfirmModal').modal('show');
    });

    // Handle delete confirm
    $('#confirmDelete').on('click', function () {
        const $btn = $(this);
        const packageId = $btn.data('id');

        $btn.html(`<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Deleting...`);
        $btn.prop('disabled', true);

        $.ajax({
            url: '/Admin/Admin/DeletePackage',
            method: 'POST',
            data: { tourPackageId: packageId },
            success: function () {
                $btn.html(`<i class="fas fa-check"></i> Deleted`);
                setTimeout(() => {
                    $('#deleteConfirmModal').modal('hide');
                    fetchAllPackages();
                }, 1000);
            },
            error: function () {
                $btn.html(`<i class="fas fa-exclamation-triangle"></i> Error`);
                setTimeout(() => {
                    $btn.html('Delete').prop('disabled', false);
                }, 2000);
            }
        });
    });

    // Initial fetch
    fetchAllPackages();
});