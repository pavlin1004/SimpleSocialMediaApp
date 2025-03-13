using Bogus;
using Microsoft.AspNetCore.Identity;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Data.Seeders;
using SimpleSocialApp.Data;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.External.AI;
using SimpleSocialApp.Services.Interfaces;
using SimpleSocialApp.Services.Implementations;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Bogus.DataSets;
using SimpleSocialApp.Data.Enums;


public class Seeder : ISeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IFriendshipService _friendshipService;
    private readonly IUserService _userService;
    private readonly IChatService _chatService;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly IReactionService _reactionService;

  
    public Seeder(UserManager<AppUser> userManager,
                IFriendshipService friendshipService,
                IUserService userService,
                IChatService chatService,
                IPostService postService,
                ICommentService commentService,
                IReactionService reactionService)
    {
        _userManager = userManager;
        _friendshipService = friendshipService;
        _userService = userService;
        _chatService = chatService;
        _postService = postService;
        _commentService = commentService;
        _reactionService = reactionService;
    }

    public async Task SeedAsync()
    {
        if (!await _userService.AnyAsync())
        {
            await SeedCustomUsers();
            await SeedRandomUsers();
            await SeedRandomFriendships();
        }

        if (!await _postService.AnyAsync())
        {
            await SeedRandomPosts();
            await SeedRandomLikesAsync();
        }
    }

    public async Task SeedCustomUsers()
    {
        await _userManager.CreateAsync(new AppUser { UserName = "123", Email ="123@abv.bg"}, "123123Aa.");

        var users = new List<AppUser>
        {
            new AppUser { FirstName = "Pavlin", LastName = "Marinov"},
            new AppUser { FirstName = "Olivia", LastName = "Smith"},
            new AppUser { FirstName = "Emma", LastName = "Williams"},
            new AppUser { FirstName = "Michael", LastName = "Brown"},
            new AppUser { FirstName = "Sophia", LastName = "Davis"}
        };

        for (int i = 0; i < users.Count; i++)
        {
            users[i].UserName = string.Concat($"Username{i}");
            users[i].Email = string.Concat("abv" + $"{i}" + "@abv.bg");
            var result = await _userManager.CreateAsync(users[i], "TestPassword123.");
            if (!result.Succeeded)
            {
                Console.WriteLine($"Error seeding user {users[i].FirstName} {users[i].LastName}");
            }
        }
    }
    public async Task SeedRandomUsers()
    {
        var userFaker = new Faker<AppUser>()
         .RuleFor(u => u.Id, () => Guid.NewGuid().ToString()) // Unique ID
         .RuleFor(u => u.FirstName, f => f.Name.FirstName())
         .RuleFor(u => u.LastName, f => f.Name.LastName())
         .RuleFor(u => u.UserName, (f, u) => $"{u.FirstName}.{u.LastName}".ToLower()) // Generate unique username
         .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
         .RuleFor(u => u.Media, () => new Media { Url = "wwwroot/images/custom_profile_picture/default_avatar.jpg", PublicId = ""});

        var users = userFaker.Generate(50);

        foreach (var user in users)
        {
            var result = await _userManager.CreateAsync(user, "DontLogIn123@");
            if (!result.Succeeded)
            {
                Console.WriteLine($"Error seeding user {user.FirstName} {user.LastName}");
            }
        }
    }

    public async Task SeedRandomFriendships()
    {
        var friendships = new HashSet<(string, string)>();
        var users = await _userService.GetAllAsync();
        var userIds = users.Select(u => u.Id).ToList();
        var faker = new Faker();
        var friendshipsCount = 0;

        while(friendshipsCount<100)
        {
            var firstId = faker.PickRandom(userIds);
            var secondId = faker.PickRandom(userIds);

            if(firstId!=secondId && !friendships.Contains((firstId,secondId)) && !friendships.Contains((secondId,firstId)))
            {
                friendships.Add((firstId,secondId));
                var friendship = new Friendship
                {
                    SenderId = firstId,
                    ReceiverId = secondId,
                    CreatedAt = DateTime.Now,
                    Type = SimpleSocialApp.Data.Enums.FriendshipType.Accepted
                };
                await _friendshipService.CreateAsync(friendship);
                var first = await _userService.GetUserByIdAsync(firstId);
                var second = await _userService.GetUserByIdAsync(secondId);
                await _chatService.CreatePrivateChatAsync(first, second);
                friendshipsCount++;
            }
        }
    }
    public async Task SeedRandomPosts()
    {
        var users = await _userService.GetAllAsync();
        var userIds = users.Select(u => u.Id);
        var postFaker = new Faker<Post>()
            .RuleFor(p => p.Id, () => Guid.NewGuid().ToString())
            .RuleFor(p => p.UserId, f => f.PickRandom(userIds))
            .RuleFor(p => p.CreatedDateTime, f => f.Date.Past())
            .RuleFor(p => p.Content, () => "  ").RuleFor(p => p.Media, () => new HashSet<Media>());

        var posts = postFaker.Generate(60);
        await _postService.AddPostsAsync(posts);
        await SeedPostContentAndImageAsync();
        
    }

    //Hardcore seeding for logically connected Content and Images (Option B: AI integration)
    public async Task SeedPostContentAndImageAsync()
    {
        var postsData = new List<(string Content, string ImageFile)>
    {
    ("Exploring the hidden gems of Paris!", "/images/posts/paris_sunset.jpeg"),
    ("Had the best sushi for dinner last night!", "/images/posts/sushi_lover.jpeg"),
    ("Weekend getaway to the mountains. Breathtaking views!", "/images/posts/mountain_views.jpeg"),
    ("Morning coffee and a good book. Perfect start to the day.", "/images/posts/coffee_morning.jpeg"),
    ("Tried a new recipe today - pasta with homemade pesto!", "/images/posts/pasta_recipe.jpeg"),
    ("A perfect day at the beach! So relaxing.", "/images/posts/beach_vibes.jpeg"),
    ("Sunset at the Grand Canyon. Words can't describe.", "/images/posts/grand_canyon_sunset.jpeg"),
    ("My cat is giving me the cold shoulder. 😒", "/images/posts/cat_mood.jpeg"),
    ("Just completed my first 5k run! Feeling proud.", "/images/posts/5k_run_finish.jpeg"),
    ("Coffee and pastries: A Sunday morning ritual.", "/images/posts/coffee_pastry.jpeg"),
    ("Lazy Sundays are the best! Just me and my couch.", "/images/posts/lazy_sunday.jpeg"),
    ("A day full of nature and fresh air. #HikingAdventures", "/images/posts/nature_hike.jpeg"),
    ("When the food looks this good, you have to share!", "/images/posts/food_lover.jpeg"),
    ("Exploring the streets of Rome. So much history!", "/images/posts/rome_exploration.jpeg"),
    ("Finally got my dream car today! So excited.", "/images/posts/dream_car.jpeg"),
    ("Loving my new workout routine. Feeling stronger every day.", "/images/posts/workout_motivation.jpeg"),
    ("Another gorgeous day in New York City. So much to see!", "/images/posts/nyc_sightseeing.jpeg"),
    ("Enjoying some self-care with a bubble bath.", "/images/posts/self_care_bath.jpeg"),
    ("I’ve never been to a place like this before. Simply magical.", "/images/posts/magical_destination.jpeg"),
    ("Delicious dinner at the new Italian restaurant in town.", "/images/posts/italian_dinner.jpeg"),
    ("Spending the day at the zoo with the kids. Such fun!", "/images/posts/zoo_day.jpeg"),
    ("Nothing like a walk in the park to clear your mind.", "/images/posts/park_walk.jpeg"),
    ("Trying out some new fashion trends. Thoughts?", "/images/posts/fashion_trends.jpeg"),
    ("My new favorite smoothie recipe - super refreshing!", "/images/posts/smoothie_time.jpeg"),
    ("Celebrating my best friend's birthday today. So much fun!", "/images/posts/birthday_bash.jpeg"),
    ("Winter wonderland vibes. Snow is falling and I love it.", "/images/posts/winter_snowfall.jpeg"),
    ("I could live at the beach forever. This view is everything.", "/images/posts/beach_sunrise.jpeg"),
    ("Saturday morning pancakes! Who else loves pancakes?", "/images/posts/pancake_morning.jpeg"),
    ("Went on a road trip this weekend. Here’s one of my favorite stops!", "/images/posts/road_trip.jpeg"),
    ("Just got back from a relaxing spa day. Highly recommend!", "/images/posts/spa_day.jpeg"),
    ("Cheers to good times with great friends!", "/images/posts/cheers_friends.jpeg"),
    ("Throwback to my favorite vacation ever!", "/images/posts/vacation_throwback.jpeg"),
    ("Can’t stop playing this game. #GamerLife", "/images/posts/gamer_life.jpeg"),
    ("Enjoying some peace and quiet by the lake.", "/images/posts/lake_relax.jpeg"),
    ("Went to the new art gallery in town. So inspiring!", "/images/posts/art_gallery.jpeg"),
    ("A lazy Sunday brunch with friends is just what I needed.", "/images/posts/brunch_buddies.jpeg"),
    ("Just finished an amazing book. Can’t wait to start another.", "/images/posts/book_night.jpeg"),
    ("Exploring the city and loving every minute of it!", "/images/posts/city_exploration.jpeg"),
    ("I love my new plant. It’s like having a piece of nature indoors.", "/images/posts/plant_love.jpeg"),
    ("Nothing beats a cozy night in with a great movie.", "/images/posts/movie_night.jpeg"),
    ("Spent the day with my favorite people. Love them to bits.", "/images/posts/friendship_love.jpeg"),
    ("Enjoying a quiet moment with my morning tea.", "/images/posts/morning_tea.jpeg"),
    ("Just finished a long hike. The view was so worth it.", "/images/posts/hike_victory.jpeg"),
    ("Weekend brunch vibes with my squad. So much laughter.", "/images/posts/brunch_squad.jpeg"),
    ("Another gorgeous sunset over the ocean.", "/images/posts/ocean_sunset.jpeg"),
    ("Making memories that will last a lifetime.", "/images/posts/memory_making.jpeg"),
    ("Just got back from an unforgettable trip to the mountains.", "/images/posts/mountain_trip.jpeg"),
    ("Life’s better with a dog. My buddy keeps me company every day.", "/images/posts/dog_lover.jpeg"),
    ("My favorite hobby: painting! Here’s my latest masterpiece.", "/images/posts/painting_art.jpeg"),
    ("I think I found the best pizza in town.", "/images/posts/best_pizza.jpeg"),
    ("Taking a moment to reflect and enjoy the peace around me.", "/images/posts/peaceful_moments.jpeg"),
    ("The best part of the weekend: relaxing at home.", "/images/posts/relaxing_at_home.jpeg"),
    ("Autumn leaves are falling. Loving the colors of fall.", "/images/posts/autumn_leaves.jpeg"),
    ("Freshly baked cookies, because why not?", "/images/posts/cookies.jpeg"),
    ("Can’t beat the vibes of a weekend in the countryside.", "/images/posts/countryside_vibes.jpeg"),
    ("A beautiful day in the garden with my plants.", "/images/posts/garden_day.jpeg"),
    ("Celebrating small wins today. Every step counts!", "/images/posts/small_win_celebration.jpeg")
    };
        var posts = await _postService.GetAllAsync();
        for (int i = 0; i < postsData.Count; i++)
        {
            posts[i].Content = postsData[i].Content;
            posts[i].Media.Add(new Media
            {
                Url = $"{postsData[i].ImageFile}",
                PublicId = "",
                Type = MediaOptions.Image,
                PostId = posts[i].Id,

            });
            await _postService.UpdatePostAsync(posts[i]);
        }
    }

    public async Task SeedRandomLikesAsync()
    {
        var users = await _userService.GetAllAsync();
        var faker = new Faker();

        foreach (var user in users)
        {
            // Get all friends of the current user
            var friends = await _friendshipService.GetAllFriendsIds(user.Id);

            // Get all posts of those friends
            var posts = await _postService.GetAllFriendsPosts(friends);

            // Shuffle and take 70% of posts
            var postsToLike = faker.Random.ListItems(posts, (int)(posts.Count * 0.7));

            foreach (var post in postsToLike)
            {
                var reaction = new Reaction
                {
                    PostId = post.Id,
                    UserId = user.Id,
                };

                await _reactionService.AddLikeAsync(reaction);
            }
        }
    }
}
