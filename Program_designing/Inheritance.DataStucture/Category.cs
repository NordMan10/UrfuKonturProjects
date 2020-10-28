using System;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public Category(string productName, MessageType messageType, MessageTopic messageTopic)
        {
            ProductName = productName;
            MessageType = messageType;
            MessageTopic = messageTopic;
        }

        public string ProductName { get; }

        public MessageType MessageType { get; }

        public MessageTopic MessageTopic { get; }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            var messageTopicValue = (int)MessageTopic + 1;
            if (Math.Abs(messageTopicValue) < 2) messageTopicValue += 3;
            var messageTypeValue = (int)MessageType + 1;
            var productNameValue = ProductName.GetHashCode() + 1;

            var result = (int)Math.Pow(messageTopicValue, messageTypeValue) * productNameValue;
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", ProductName, MessageType, MessageTopic);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Category category)) return 1;
            if (ProductName != category.ProductName) 
                return ProductName == null ? -1 : ProductName.CompareTo(category.ProductName);
            if (MessageType != category.MessageType) 
                return MessageType.CompareTo(category.MessageType);
            return MessageTopic.CompareTo(category.MessageTopic);
        }

        public static bool operator ==(Category thisCategory, Category otherCategory)
        {
            return thisCategory.CompareTo(otherCategory) == 0;
        }

        public static bool operator !=(Category thisCategory, Category otherCategory)
        {
            return thisCategory.CompareTo(otherCategory) != 0;
        }

        public static bool operator >(Category thisCategory, Category otherCategory)
        {
            return thisCategory.CompareTo(otherCategory) > 0;
        }

        public static bool operator <(Category thisCategory, Category otherCategory)
        {
            return thisCategory.CompareTo(otherCategory) < 0;
        }

        public static bool operator >=(Category thisCategory, Category otherCategory)
        {
            return thisCategory.CompareTo(otherCategory) >= 0;
        }

        public static bool operator <=(Category thisCategory, Category otherCategory)
        {
            return thisCategory.CompareTo(otherCategory) <= 0;
        }
    }
}
