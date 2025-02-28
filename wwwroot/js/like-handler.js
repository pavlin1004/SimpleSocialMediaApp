$(document).ready(function () {
    // Handle like/unlike button click
    $('.like-button').click(function () {
        var targetType = $(this).data('target-type');
        var targetId = $(this).data('target-id');
        var icon = $(this).find('svg'); // Find the SVG icon inside the button

        // Send AJAX request to toggle like/unlike
        $.ajax({
            url: '/Reaction/ToggleLike', // Change the URL to match the correct path in your app
            type: 'POST',
            data: {
                targetType: targetType,
                targetId: targetId
            },
            success: function (response) {
                if (response.success) {
                    // Update the like count
                    $('#like-count-' + targetId).text(response.newLikeCount);

                    // Toggle the SVG color based on like status
                    if (response.isLiked) {
                        icon.css('fill', 'blue'); // Change to blue when liked
                    } else {
                        icon.css('fill', 'currentColor'); // Revert back to the default color when unliked
                    }
                } else {
                    alert('An error occurred: ' + response.message);
                }
            },
            error: function () {
                alert('Error while processing your request');
            }
        });
    });
});
