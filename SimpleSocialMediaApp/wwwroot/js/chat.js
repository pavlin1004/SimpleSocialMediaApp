document.addEventListener("DOMContentLoaded", function () {
    var chatId = document.getElementById("chatId").value;
    var chatBox = document.getElementById("chatBox");

    var isGroupChat = document.getElementById("isGroupChat").value === "true"; 
    // Create a SignalR connection
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub")
        .build();

    // Start the connection
    connection.start().then(() => {
        console.log("Connected to SignalR");
        connection.invoke("JoinChat", chatId);
    }).catch(err => console.error("Error starting SignalR:", err));

    // Listen for incoming messages
    connection.on("ReceiveMessage", function (userName, message, timestamp) {
        // Create new message element
        var newMessage = document.createElement("div");
        newMessage.classList.add("right-paragraph");
        newMessage.innerHTML = `
        <p class="${userName === 'CurrentUserName' ? 'right-paragraph' : 'left-paragraph'}">${message}</p>
         `;

        // Prepend the new message at the top
        chatBox.appendChild(newMessage, chatBox.firstChild);

        // Scroll to the top of the chat box
        chatBox.scrollTop = chatBox.scrollHeight;
    });

    // Handle message form submission
    document.getElementById("messageForm").addEventListener("submit", function (event) {
        event.preventDefault(); // Prevent page reload

        var messageContent = document.getElementById("messageInput").value;
        var chatIdElement = document.getElementById("chatId"); // Get the hidden input

        if (chatIdElement == null) {
            console.error("Chat ID is missing!");
            return;
        }

        var chatId = chatIdElement.value;

        fetch("/Message/Send", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Content: messageContent, ChatId: chatId })
        })
            .then(response => {
                if (response.ok) {
                    document.getElementById("messageInput").value = ""; // Clear input field
                } else {
                    console.error("Error sending message:", response.status);
                }
            })
            .catch(err => console.error("Fetch error:", err));
    });
});
// Get the elements
const moreOptionsBtn = document.getElementById('moreOptionsBtn');
const buttonContainer = document.querySelector('.button-container');

// Add event listener to toggle visibility
moreOptionsBtn.addEventListener('click', function () {
    // Toggle the display of the button container
    if (buttonContainer.style.display === 'none' || buttonContainer.style.display === '') {
        buttonContainer.style.display = 'flex'; // Show the buttons
    } else {
        buttonContainer.style.display = 'none'; // Hide the buttons
    }
});
