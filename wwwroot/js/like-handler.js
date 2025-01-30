// like-handler.js
$(document).ready(function () {
    // Handle like/unlike button click
    $('.like-button').click(function () {
        var targetType = $(this).data('target-type');
        var targetId = $(this).data('target-id');

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

                    // Toggle the button text (Like/Unlike)
                    var button = $('[data-target-id="' + targetId + '"]');
                    if (response.isLiked) {
                        button.text('Unlike');
                    } else {
                        button.text('Like');
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
