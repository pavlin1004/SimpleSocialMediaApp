// Initialize count for messages starting from 1
let count = 1;
const chatId = document.getElementById('ChatId').value;
const messagesContainer = document.getElementById('messagesContainer');
const chatBox = document.getElementById('chatBox');

// Function to load more messages
async function loadMoreMessages() {
    const response = await fetch(`/Chat/Index?chatId=${chatId}&count=${count}&size=5`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (response.ok) {
        const data = await response.text();
        if (data.trim() === '') {
            // If no messages are returned, you can handle it here
            console.log('No more messages to load.');
        } else {
            // Store the current scroll position
            const previousScrollTop = chatBox.scrollTop;

            // Get the height of the messages container before adding new messages
            const previousHeight = messagesContainer.scrollHeight;

            // Prepend the new messages to the top of the messages container
            messagesContainer.insertAdjacentHTML('beforeend', data);
            count++; // Increment count after successfully loading messages

            // Calculate the new height of the messages container after adding new messages
            const newHeight = messagesContainer.scrollHeight;

            // Adjust the scroll position to keep the view consistent
            chatBox.scrollTop = previousScrollTop + (newHeight - previousHeight);
        }
    } else {
        console.error('Failed to load messages:', response.status);
    }
}

// Function to scroll to the bottom of the chat box
function scrollToBottom() {
    chatBox.scrollTop = chatBox.scrollHeight;
}

// Event listener for scroll event
chatBox.addEventListener('scroll', function () {
    if (chatBox.scrollTop === 0) {
        loadMoreMessages(); // Load more messages when scrolled to the top
    }
});

// Scroll to the bottom when the page loads
window.onload = function () {
    scrollToBottom();
};