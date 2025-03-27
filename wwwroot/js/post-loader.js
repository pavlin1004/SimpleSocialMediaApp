let size = 5;  // Number of messages per request
let count = 1; // Track how many batches of messages have been loaded
let isLoading = false; // Prevent multiple simultaneous requests

const chatId = document.getElementById("chatId").value; // Chat ID

// Function to load messages using the provided URL
function loadMessages(url) {
    if (isLoading) return; // Prevent duplicate calls
    isLoading = true;

    // Make a fetch request to load messages (or other content)
    fetch(`${url}?chatId=${chatId}&size=${size}&count=${count}`)
        .then(response => response.text())  // Expect HTML (partial view)
        .then(html => {
            // Append the new messages to the existing chat container
            document.querySelector('.chat-box').insertAdjacentHTML('beforeend', html);

            // Increment count for the next batch
            count++;

            // Allow new requests after messages are added
            isLoading = false;

            // Optionally, hide or remove the "load more" button if there are no more messages
            if (html.trim() === '') {
                // No more messages to load, hide the "load more" button
                const loadMoreTrigger = document.getElementById('load-more-trigger');
                loadMoreTrigger.style.display = 'none';
            }
        })
        .catch(error => {
            console.error('Error loading messages:', error);
            isLoading = false;
        });
}

// Function to check if the user has scrolled near the bottom of the chat box
function isNearBottom() {
    const chatBox = document.getElementById('chatBox');
    return chatBox.scrollTop + chatBox.clientHeight >= chatBox.scrollHeight - 100;
}

// Add scroll event listener to load more messages when near the bottom of the chat box
document.getElementById('chatBox').addEventListener('scroll', () => {
    const chatBox = document.getElementById('chatBox');
    if (isNearBottom() && !isLoading) {
        const loadMoreUrl = document.getElementById('load-more-trigger').getAttribute('data-url');
        loadMessages(loadMoreUrl); // Call the function to load more messages
    }
});

// Initial load of the first batch of messages when the page is loaded
window.addEventListener('DOMContentLoaded', () => {
    // Get the URL from the data-url attribute of the load-more-trigger element
    const loadMoreUrl = document.getElementById('load-more-trigger').getAttribute('data-url');
    loadMessages(loadMoreUrl); // Call the function with the dynamic URL
});
