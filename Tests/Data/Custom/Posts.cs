using SimpleSocialApp.Data.Models;

namespace Tests.Data.Custom
{
    public static class Posts
    {
        public const string postParam1 = "testPost1";
        public const string postParam2 = "testPost2";
        public const string postParam3 = "testPost3";

        public const string mediaParam1 = "testMedia1";
        public const string mediaParam2 = "testMedia2";

        public const string commentParam1 = "commentParam1";

        public static readonly Post Post1 = new Post
        {
            Id = postParam1,
            Content = postParam1,
            User = Users.User2,
            Media = new List<Media>
            {
                new Media {
                    Id = "testMedia1",
                    Url = "testMedia1",
                    PublicId = "testMedia1"
                    },
            },
            Comments = new List<Comment>
            {
                new Comment
                {
                    Content = commentParam1,
                    User = Users.User4
                }
            },
            Reacts = new List<Reaction>
            {
                new Reaction
                {
                    User = Users.User4
                }
            }
        };

        public static readonly Post Post2 = new Post
        {
            Id = postParam2,
            Content = postParam2,
            Media = new List<Media>
            {
                new Media {
                    Id = "testMedia2",
                    Url = "testMedia2",
                    PublicId = "testMedia2"
                    }
            },
            User = Users.User2
        };



    }
}
