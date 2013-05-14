using System.ComponentModel.DataAnnotations.Schema;

namespace Massive.DAL.Models
{
    /// <summary>
    /// A class which represents the PostTypes table.
    /// </summary>
    public class PostTypes : DynamicModel 
    {
        public PostTypes(string connectionStringName,
            string tableName = "PostTypes", 
            string primaryKeyField = "Id", 
            string descriptorField = "") : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the PostsTags table.
    /// </summary>
    public class PostsTags : DynamicModel 
    {
        public PostsTags(string connectionStringName,
            string tableName = "PostsTags", 
            string primaryKeyField = "Id", 
            string descriptorField = "") : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the Badges table.
    /// </summary>
    public class Badges : DynamicModel
    {
        public Badges(string connectionStringName,
            string tableName = "Badges", string primaryKeyField = "Id", string descriptorField = "")
            : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }

      
    }

    /// <summary>
    /// A class which represents the Comments table.
    /// </summary>
    public class Comments :DynamicModel
    {
        public Comments(string connectionStringName,
            string tableName = "Comments", string primaryKeyField = "Id", string descriptorField = "")
            : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the Posts table.
    /// </summary>
    public class Posts : DynamicModel
    {
        public Posts(string connectionStringName,
            string tableName = "Posts", 
            string primaryKeyField = "Id", string descriptorField = "") 
            : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the Users table.
    /// </summary>
    public class Users :DynamicModel
    {
        public Users(string connectionStringName
            , string tableName = "Users", 
            string primaryKeyField = "Id", string descriptorField = "") : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the Votes table.
    /// </summary>
    public class Votes :DynamicModel
    {
        public Votes(string connectionStringName, string tableName = "Votes", string primaryKeyField = "Id", string descriptorField = "")
            : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the Tags table.
    /// </summary>
    public class Tags : DynamicModel
    {
        public Tags(string connectionStringName, string tableName = "Tags", string primaryKeyField = "Id", string descriptorField = "")
            : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the PostTags table.
    /// </summary>
    public class PostTags : DynamicModel
    {
        public PostTags(string connectionStringName, string tableName = "PostTags", string primaryKeyField = "Id", string descriptorField = "")
            : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

    /// <summary>
    /// A class which represents the VoteTypes table.
    /// </summary>
    public class VoteTypes : DynamicModel
    {
        public VoteTypes(string connectionStringName, string tableName = "VoteTypes", string primaryKeyField = "Id", string descriptorField = "") 
            : base(connectionStringName, tableName, primaryKeyField, descriptorField)
        {
        }
    }

}