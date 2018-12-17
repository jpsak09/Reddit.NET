﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reddit.NET;
using Reddit.NET.Controllers;
using Reddit.NET.Controllers.EventArgs;
using Reddit.NET.Exceptions;
using RedditThings = Reddit.NET.Models.Structures;
using System;
using System.Collections.Generic;

namespace Reddit.NETTests.ControllerTests.WorkflowTests
{
    [TestClass]
    public class AccountTests : BaseTests
    {
        public AccountTests() : base() { }

        [TestMethod]
        public void Friendship()
        {
            User patsy = GetTargetUser();

            // Add a friend.
            Validate(reddit.Account.UpdateFriend(patsy.Name));

            // Get data on an existing friend.
            Validate(reddit.Account.GetFriend(patsy.Name));

            // It's just not working out.  Delete the friend and burn all their stuff.
            reddit.Account.DeleteFriend(patsy.Name);
        }
    }
}
