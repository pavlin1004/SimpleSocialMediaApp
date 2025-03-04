$(document).ready(function () {
    // Handle like/unlike button click
    $('.like-button').click(function () {
        var targetType = $(this).data('target-type');
        var targetId = $(this).data('target-id');
        var icon = $(this).find('svg'); // Find the SVG icon inside the button

        // Send AJAX request to toggle like/unlike
        $.ajax({
            url: '/Reaction/ToggleLike', // Ensure this is the correct URL in your app
            type: 'POST',
            data: {
                targetType: targetType,
                targetId: targetId
            },
            success: function (response) {
                if (response.success) {
                    // Debugging: Check what the like count element contains before and after
                    var likeText = $('#like-count-' + targetId).text();
                    console.log("Before update: " + likeText);  // Log the current text

                    // Check the response and set the updated like count
                    var updatedText = "Likes: " + response.newLikeCount; // Updated text with like count
                    console.log("After update: " + updatedText);  // Log the updated text

                    // Update the like count text (keeping "Likes:" and just changing the number)
                    $('#like-count-' + targetId).text(updatedText);

                    // Optionally: Toggle the icon's color
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
