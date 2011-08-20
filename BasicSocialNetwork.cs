using System;
using Sider;  //https://github.com/chakrit/sider

namespace SimpleRedisExamples
{
	public class BasicSocialNetwork
	{
		
		RedisClient Client;
		
		public BasicSocialNetwork ()
		{
			Client = new RedisClient("localhost", 6379);
			
			//CreateDemoData();
		}
		
		public long CreateNewUser(string userName)
		{
			long uid = Client.Incr("next_user_id");
			Client.Set(string.Format("user:{0}:name", uid),userName);
			Client.Set(string.Format("username:{0}", userName), uid.ToString());
			return uid;
		}
		
		public long CreateNewPost(string postContent, long uid)
		{
			long pid = Client.Incr("next_post_id");
			Client.Set(string.Format("post:{0}:content", pid),postContent);
			Client.Set(string.Format("post:{0}:user", pid), uid.ToString());
			Client.LPush(string.Format("user:{0}:posts", uid), pid.ToString());
			Client.LPush("posts:global", pid.ToString());
			return pid;
		}

		public void FollowUser(long followerId, long watchedId)
		{
			Client.SAdd(string.Format("user:{0}:follows", followerId), 
			            watchedId.ToString());
			Client.SAdd(string.Format("user:{0}followed_by", watchedId), 
			            followerId.ToString());
		}
		
/*		private void CreateDemoData()
		{
			var uid = CreateNewUser("Fred");
			var uname = Client.Get(string.Format(KEY_USER_NAME, uid));
			Console.WriteLine(string.Format("{0}'s user id is {1}",uname, uid)); 
			
			var pid = CreateNewPost("Devlink Rocks!", uid);
			var content = Client.Get(string.Format(KEY_POST_CONTENT, pid));
			var owner = Client.Get(string.Format(KEY_POST_USER_ID, pid));
			Console.WriteLine(string.Format(
				"Post {0} belongs to User {1} and says \"{2}\"",
			    pid, owner, content));
			
		}
		*/
	
	}
}

