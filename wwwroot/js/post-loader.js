let size = 5;  // Number of posts per request
let count = 1; // Track how many batches of posts have been loaded
let isLoading = false; // Prevent multiple simultaneous requests

// Function to load posts using the provided URL
function loadPosts(url, userId = null) {
    if (isLoading) return; // Prevent duplicate calls
    isLoading = true;

    // Construct URL with necessary parameters
    let requestUrl = `${url}?size=${size}&count=${count}`;

    // If userId is provided, append it to the request
    if (userId) {
        requestUrl += `&userId=${userId}`;
    }

    // Make a fetch request
    fetch(requestUrl)
        .then(response => response.text())  // Expect HTML (partial view)
        .then(html => {
            // Append the new posts to the existing posts section
            document.querySelector('.posts-section').insertAdjacentHTML('beforeend', html);

            // Increment count for the next batch
            count++;

            // Allow new requests after posts are added
            isLoading = false;
        })
        .catch(error => {
            console.error('Error loading posts:', error);
            isLoading = false;
        });
}

// Function to check if the user has scrolled near the bottom of the page
function isNearBottom() {
    return window.innerHeight + window.scrollY >= document.body.offsetHeight - 100;
}

window.addEventListener('scroll', () => {
    if (isNearBottom() && !isLoading) {
        const loadMoreUrl = document.getElementById('load-more-trigger').getAttribute('data-url');
        const loadUserId = document.getElementById('load-more-trigger').getAttribute('data-userId');
        loadPosts(loadMoreUrl, loadUserId);  // ✅ Now passing userId
    }
});

window.addEventListener('DOMContentLoaded', () => {
    const loadMoreUrl = document.getElementById('load-more-trigger').getAttribute('data-url');
    const loadUserId = document.getElementById('load-more-trigger').getAttribute('data-userId');
    loadPosts(loadMoreUrl, loadUserId);  // ✅ Now passing userId
});