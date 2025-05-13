$(document).ready(function () {

    // Function to fetch and display recommended tour packages
    function fetchRecommendedPackages() {
        // Make an AJAX request to the recommendations endpoint
        $.ajax({
            url: '/Home/GetRecommendationsForCurrentUser', // Endpoint for recommendations for the current user
            method: 'GET', // Use GET method to retrieve data
            dataType: 'json', // Expect JSON data in the response
            success: function (data) {
                // This function is called if the AJAX request is successful
                let html = ''; // Initialize an empty string to build HTML

                // Check if data is an array and has items
                if (Array.isArray(data) && data.length > 0) {
                    // Loop through each recommended package received in the data
                    data.forEach(function (pkg) {
                        // Build the HTML structure for a single tour card
                        html += `
                            <div class="tour-card shadow" data-id="${pkg.tourPackageId}" style="cursor:pointer;">
                                <img src="${pkg.destinationImage}" class="card-img-top" alt="${pkg.name}">
                                <div class="card-body">
                                    <h5 class="card-title">${pkg.name}</h5>
                                    <h5 class="card-title">${pkg.numberOfDays} days</h5> {/* Added 'days' for clarity */}
                                    <div class="description">${pkg.description.substring(0, 80)}...</div> {/* Display truncated description */}
                                    <p class="card-text"><strong>Destination:</strong> ${pkg.destination}</p>
                                    <p class="card-text"><strong>Rating:</strong> ${pkg.rating}</p>
                                    <p class="card-text"><strong>Price:</strong> ₹${pkg.price}</p>
                                </div>
                            </div>`;
                    });
                } else {
                    // If no recommendations are returned, display a message
                    html = '<div class="alert alert-info">No recommendations found based on your latest activity.</div>';
                }

                // Set the generated HTML content into the package container
                $('#packageContainer').html(html);

                // Initialize the scroll buttons after the content is loaded
                initScrollButtons();
            },
            error: function (xhr, status, error) {
                // This function is called if the AJAX request fails
                console.error("Error fetching recommendations:", status, error); // Log the error to the console
                // Display an error message to the user
                $('#packageContainer').html('<div class="alert alert-danger">Could not load tour package recommendations.</div>');
            }
        });
    }

    // Call the function to fetch and display recommendations when the document is ready
    fetchRecommendedPackages();

    // --- Existing code for handling tour card clicks and scroll buttons ---

    // Use event delegation for click events on tour cards.
    // This is efficient as it attaches one listener to a parent element
    // and handles clicks on any current or future .tour-card elements.
    $(document).on('click', '.tour-card', function () {
        // Get the tour package ID from the data-id attribute of the clicked card
        const id = $(this).data('id');
        console.log('Navigating to package ID:', id); // Debug log
        // Navigate to the booking page with the package ID
        window.location.href = `/UserActivity/User/Booking?id=${id}`;
    });

    // Function to initialize and manage the state of the horizontal scroll buttons
    function initScrollButtons() {
        const container = document.getElementById('packageContainer');
        const scrollLeftBtn = document.getElementById('scrollLeft');
        const scrollRightBtn = document.getElementById('scrollRight');

        // Exit if container or buttons are not found
        if (!container || !scrollLeftBtn || !scrollRightBtn) return;

        // Define the width of a single card and the amount to scroll by
        const cardWidth = 320; // Approximate width of a tour card
        const scrollAmount = cardWidth * 3; // Scroll by a few card widths at a time

        // Add event listener for the left scroll button
        scrollLeftBtn.addEventListener('click', function () {
            container.scrollLeft -= scrollAmount; // Scroll left
            updateButtonState(); // Update button disabled state
        });

        // Add event listener for the right scroll button
        scrollRightBtn.addEventListener('click', function () {
            container.scrollLeft += scrollAmount; // Scroll right
            updateButtonState(); // Update button disabled state
        });

        // Add event listener to the container's scroll event
        // This updates button state when the user scrolls manually (e.g., with mouse wheel or touch)
        container.addEventListener('scroll', updateButtonState);

        // Function to update the disabled state of the scroll buttons
        // based on the current scroll position.
        function updateButtonState() {
            // Disable left button if scrolled all the way to the left
            scrollLeftBtn.disabled = container.scrollLeft <= 0;
            // Calculate the maximum possible scroll position
            const maxScrollLeft = container.scrollWidth - container.clientWidth;
            // Disable right button if scrolled all the way to the right (allow a small tolerance)
            scrollRightBtn.disabled = Math.abs(container.scrollLeft - maxScrollLeft) < 10; // Use tolerance for floating point comparison
        }

        // Call updateButtonState initially to set the correct state on page load
        updateButtonState();
    }

    // Note: The original AJAX call to '/Home/TourPackage' has been replaced
    // by the call to fetchRecommendedPackages(). If you need to display
    // all packages elsewhere, you would add a separate function and call it.

});
