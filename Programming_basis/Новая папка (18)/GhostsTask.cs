using System;
using System.Text;

namespace hashes
{
	public class GhostsTask : 
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
		IMagic
	{

        public bool IsVector { get; set; }
        public bool IsSegment { get; set; }
        public bool IsDocument { get; set; }
        public bool IsCat { get; set; }
        public bool IsRobot { get; set; }

		public void DoMagic()
		{
            if (Vector.X == 0) 
		}

		// Чтобы класс одновременно реализовывал интерфейсы IFactory<A> и IFactory<B> 
		// придется воспользоваться так называемой явной реализацией интерфейса.
		// Чтобы отличать методы создания A и B у каждого метода Create нужно явно указать, к какому интерфейсу он относится.
		// На самом деле такое вы уже видели, когда реализовывали IEnumerable<T>.

		Vector IFactory<Vector>.Create()
		{
            IsVector = true;
            return new Vector(0, 0);
		}

		Segment IFactory<Segment>.Create()
		{
            IsSegment = true;
            return new Segment(new Vector(0, 0), new Vector(0, 0));
		}

        Document IFactory<Document>.Create()
        {
            IsDocument = true;
            return new Document("Title", Encoding.ASCII, new byte[0]);
        }
        
        Cat IFactory<Cat>.Create()
        {
            IsCat = true;
            return new Cat("CatName", "Breed", new DateTime(2010, 10, 1)); ;
        }

        Robot IFactory<Robot>.Create()
        {

            return new Robot("0");
        }

    }
}