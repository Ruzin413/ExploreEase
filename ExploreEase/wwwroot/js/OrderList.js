$(function () {
    let selectedRange = null;

    // Initialize Flatpickr for date range selection
    flatpickr("#dateRange", {
        mode: "range",
        dateFormat: "Y-m-d",
        onChange: function (selectedDates) {
            selectedRange = selectedDates.length === 2 ? selectedDates : null;
            fetchAndRenderOrders();
        }
    });

    function formatDate(date) {
        const d = new Date(date);
        return d.toISOString().split('T')[0];
    }

    function isInRange(dateStr) {
        if (!selectedRange) return true;
        const date = new Date(dateStr);
        return date >= selectedRange[0] && date <= selectedRange[1];
    }

    function renderOrders(orders, tbodySelector) {
        const tbody = $(tbodySelector);
        tbody.empty();

        let count = 0;
        orders.forEach(order => {
            // Check if order start or end date is in range filter
            if (!isInRange(order.startDate) && !isInRange(order.endDate)) return;

            const row = $(`
                        <tr id="row-${order.id}">
                            <td>${order.username}</td>
                            <td>${order.price}</td>
                            <td>${formatDate(order.startDate)}</td>
                            <td>${formatDate(order.endDate)}</td>
                            <td>${formatDate(order.bookingDate)}</td>
                            <td>${order.numberOfPeople}</td>
                            <td>${order.totalPrice}</td>
                            <td><button class="btn btn-delete delete-btn" data-id="${order.id}">Delete</button></td>
                            <td><button class="btn btn-detail detail-btn" data-id="${order.id}">Detail</button></td>
                        </tr>
                    `);
            tbody.append(row);
            count++;
        });

        if (count === 0) {
            tbody.html('<tr><td colspan="9" class="empty-state">No orders found</td></tr>');
        }
    }

    function fetchAndRenderOrders() {
        $.ajax({
            url: "/Admin/Admin/OrderList1",
            method: 'GET',
            success: function (data) {
                const now = new Date();
                const ongoing = [];
                const upcoming = [];
                const done = [];

                data.forEach(order => {
                    const start = new Date(order.startDate);
                    const end = new Date(order.endDate);

                    if (start <= now && now <= end) {
                        ongoing.push(order);
                    } else if (start > now) {
                        upcoming.push(order);
                    } else {
                        done.push(order);
                    }
                });

                renderOrders(ongoing, '#ongoingOrdersBody');
                renderOrders(upcoming, '#upcomingOrdersBody');
                renderOrders(done, '#doneOrdersBody');
            },
            error: function () {
                $('.order-table tbody').html('<tr><td colspan="9" class="empty-state">Failed to load orders</td></tr>');
            }
        });
    }

    // Initial fetch
    fetchAndRenderOrders();

    // Delete button
    $(document).on('click', '.delete-btn', function () {
        const orderId = $(this).data('id');
        if (confirm('Are you sure you want to delete this order?')) {
            $.ajax({
                url: `/Admin/Admin/DeleteOrder/${orderId}`,
                method: 'DELETE',
                success: function () {
                    $(`#row-${orderId}`).remove();
                },
                error: function () {
                    alert('Failed to delete order');
                }
            });
        }
    });

    // Detail button
    $(document).on('click', '.detail-btn', function () {
        const orderId = $(this).data('id');
        window.location.href = `/Admin/Admin/OrderDetail/${orderId}`;
    });

    // Tabs switching logic
    const tabs = $('.tab');
    const panels = $('.tab-panel');

    tabs.on('click keydown', function (e) {
        if (e.type === 'click' || (e.type === 'keydown' && (e.key === 'Enter' || e.key === ' '))) {
            e.preventDefault();

            const selectedTab = $(this);
            const targetId = selectedTab.attr('aria-controls');

            tabs.removeClass('active').attr({
                'aria-selected': 'false',
                tabindex: -1
            });
            panels.attr('hidden', true);

            selectedTab.addClass('active').attr({
                'aria-selected': 'true',
                tabindex: 0
            }).focus();
            $(`#${targetId}`).removeAttr('hidden');
        }
    });

    // Keyboard navigation between tabs (Left/Right arrows)
    tabs.on('keydown', function (e) {
        const currentIndex = tabs.index(this);
        if (e.key === 'ArrowRight' || e.key === 'ArrowDown') {
            e.preventDefault();
            const nextIndex = (currentIndex + 1) % tabs.length;
            tabs.eq(nextIndex).focus();
        } else if (e.key === 'ArrowLeft' || e.key === 'ArrowUp') {
            e.preventDefault();
            const prevIndex = (currentIndex - 1 + tabs.length) % tabs.length;
            tabs.eq(prevIndex).focus();
        }
    });
});