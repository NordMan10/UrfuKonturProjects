using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        public List<T> Items = new List<T>();
        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (Items)
                {
                    if (index < Items.Count) return Items[index];
                    else return null;
                }
            }
            set
            {
                lock (Items)
                {
                    if (index > Items.Count) throw new IndexOutOfRangeException();
                    if (index == Items.Count) Items.Add(value);
                    else
                    {
                        for (var i = Items.Count - 1; i > index; i--)
                        {
                            Items.RemoveAt(i);
                        }
                        Items[index] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (Items)
            {
                if (Items.Count == 0) return null;
                return Items[Items.Count - 1];
            }
        }

        

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (Items)
            {
                if (Items.Count == 0)
                    if (knownLastItem == null) Items.Add(item);
                var currentLastItem = Items[Items.Count - 1];
                if (currentLastItem.Equals(knownLastItem))
                    Items.Add(item);
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (Items)
                {
                    return Items.Count;
                }
            }
        }
    }
}