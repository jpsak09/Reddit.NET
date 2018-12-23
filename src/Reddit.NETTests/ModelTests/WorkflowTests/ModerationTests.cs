﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reddit.NET.Models.Structures;

namespace Reddit.NETTests.ModelTests.WorkflowTests
{
    [TestClass]
    public class ModerationTests : BaseTests
    {
        public ModerationTests() : base() { }

        // Requires existing subreddit with mod privilages.  --Kris
        [TestMethod]
        public void Approve()
        {
            Post post = reddit.Models.Listings.New(null, null, true, testData["Subreddit"]).Data.Children[0].Data;

            reddit.Models.Moderation.Approve(post.Name);

            post = reddit.Models.LinksAndComments.Info(post.Name).Posts[0];

            Assert.IsNotNull(post);
            Assert.IsTrue(post.Approved);
        }

        [TestMethod]
        public void ModeratorInvite()
        {
            User patsy = GetTargetUserModel();

            Validate(patsy);

            GenericContainer res = reddit.Models.Users.Friend(null, null, null, null, 999, patsy.Name, "+mail", "moderator_invite", testData["Subreddit"]);

            Validate(res);

            res = reddit2.Models.Moderation.AcceptModeratorInvite(testData["Subreddit"]);

            Validate(res);

            reddit2.Models.Moderation.LeaveModerator(reddit2.Models.Subreddits.About(testData["Subreddit"]).Data.Name);
        }

        [TestMethod]
        public void Distinguish()
        {
            PostResultShortContainer postResult = TestPost();

            Validate(postResult);

            CommentResultContainer commentResult = TestComment(postResult.JSON.Data.Name);

            Validate(commentResult);

            PostResultContainer post = reddit.Models.Moderation.DistinguishPost("yes", postResult.JSON.Data.Name);
            CommentResultContainer comment = reddit.Models.Moderation.DistinguishComment("yes", commentResult.JSON.Data.Things[0].Data.Name, true);

            Validate(post);
            Assert.AreEqual(postResult.JSON.Data.Name, post.JSON.Data.Things[0].Data.Name);
            Assert.AreEqual("moderator", post.JSON.Data.Things[0].Data.Distinguished);

            Validate(comment);
            Assert.AreEqual(commentResult.JSON.Data.Things[0].Data.Name, comment.JSON.Data.Things[0].Data.Name);
            Assert.AreEqual("moderator", comment.JSON.Data.Things[0].Data.Distinguished);
            Assert.IsTrue(comment.JSON.Data.Things[0].Data.Stickied);
        }

        [TestMethod]
        public void IgnoreReports()
        {
            PostResultShortContainer postResult = TestPost();

            Validate(postResult);

            CommentResultContainer commentResult = TestComment(postResult.JSON.Data.Name);

            Validate(commentResult);

            reddit.Models.Moderation.IgnoreReports(postResult.JSON.Data.Name);
            reddit.Models.Moderation.IgnoreReports(commentResult.JSON.Data.Things[0].Data.Name);

            reddit.Models.Moderation.UnignoreReports(postResult.JSON.Data.Name);
            reddit.Models.Moderation.UnignoreReports(commentResult.JSON.Data.Things[0].Data.Name);
        }

        [TestMethod]
        public void Contributor()
        {
            User patsy = GetTargetUserModel();

            Validate(patsy);

            GenericContainer res = reddit.Models.Users.Friend(null, null, null, null, 999, patsy.Name, "", "contributor", testData["Subreddit"]);

            Validate(res);

            reddit2.Models.Moderation.LeaveContributor(reddit2.Models.Subreddits.About(testData["Subreddit"]).Data.Name);
        }

        [TestMethod]
        public void Remove()
        {
            PostResultShortContainer postResult = TestPost();

            Validate(postResult);

            CommentResultContainer commentResult = TestComment(postResult.JSON.Data.Name);

            Validate(commentResult);

            reddit.Models.Moderation.Remove(postResult.JSON.Data.Name, false);
            reddit.Models.Moderation.Remove(commentResult.JSON.Data.Things[0].Data.Name, false);
        }
    }
}