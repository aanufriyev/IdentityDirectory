namespace Klaims.Framework.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;

	// From EF7
	public class SequentialGuidValueGenerator 
	{
		private long counter = DateTime.UtcNow.Ticks;

		public Guid Next()
		{
			var guidBytes = Guid.NewGuid().ToByteArray();
			var counterBytes = BitConverter.GetBytes(Interlocked.Increment(ref this.counter));

			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(counterBytes);
			}

			guidBytes[08] = counterBytes[1];
			guidBytes[09] = counterBytes[0];
			guidBytes[10] = counterBytes[7];
			guidBytes[11] = counterBytes[6];
			guidBytes[12] = counterBytes[5];
			guidBytes[13] = counterBytes[4];
			guidBytes[14] = counterBytes[3];
			guidBytes[15] = counterBytes[2];

			return new Guid(guidBytes);
		}

		public bool GeneratesTemporaryValues => false;
	}
	// From http://www.siepman.nl/blog/post/2013/10/28/ID-Sequential-Guid-COMB-Vs-Int-Identity-using-Entity-Framework.aspx
	public class SequentialGuid
	{
		private const int NumberOfBytes = 6;

		private const int PermutationsOfAByte = 256;

		private static readonly Lazy<SequentialGuid> InstanceField = new Lazy<SequentialGuid>(() => new SequentialGuid());

		private readonly long maximumPermutations = (long)Math.Pow(PermutationsOfAByte, NumberOfBytes);

		private readonly object synchronizationObject = new object();

		private long lastSequence;

		public SequentialGuid(DateTime sequenceStartDate, DateTime sequenceEndDate)
		{
			this.SequenceStartDate = sequenceStartDate;
			this.SequenceEndDate = sequenceEndDate;
		}

		public SequentialGuid()
			: this(new DateTime(2011, 10, 15), new DateTime(2100, 1, 1))
		{
		}

		public DateTime SequenceStartDate { get; }

		public DateTime SequenceEndDate { get; }

		internal static SequentialGuid Instance => InstanceField.Value;

		public TimeSpan TimePerSequence
		{
			get
			{
				var ticksPerSequence = this.TotalPeriod.Ticks / this.maximumPermutations;
				var result = new TimeSpan(ticksPerSequence);
				return result;
			}
		}

		public TimeSpan TotalPeriod
		{
			get
			{
				var result = this.SequenceEndDate - this.SequenceStartDate;
				return result;
			}
		}

		public static Guid NewGuid()
		{
			return Instance.GetGuid();
		}

		private long GetCurrentSequence(DateTime value)
		{
			var ticksUntilNow = value.Ticks - this.SequenceStartDate.Ticks;
			var result = ((decimal)ticksUntilNow / this.TotalPeriod.Ticks * this.maximumPermutations - 1);
			return (long)result;
		}

		public Guid GetGuid()
		{
			return this.GetGuid(DateTime.Now);
		}

		internal Guid GetGuid(DateTime now)
		{
			if (now < this.SequenceStartDate || now > this.SequenceEndDate)
			{
				return Guid.NewGuid(); // Outside the range, use regular Guid
			}

			var sequence = this.GetCurrentSequence(now);
			return this.GetGuid(sequence);
		}

		internal Guid GetGuid(long sequence)
		{
			lock (this.synchronizationObject)
			{
				if (sequence <= this.lastSequence)
				{
					// Prevent double sequence on same server
					sequence = this.lastSequence + 1;
				}
				this.lastSequence = sequence;
			}

			var sequenceBytes = this.GetSequenceBytes(sequence);
			var guidBytes = this.GetGuidBytes();
			var totalBytes = guidBytes.Concat(sequenceBytes).ToArray();
			var result = new Guid(totalBytes);
			return result;
		}

		private IEnumerable<byte> GetSequenceBytes(long sequence)
		{
			var sequenceBytes = BitConverter.GetBytes(sequence);
			var sequenceBytesLongEnough = sequenceBytes.Concat(new byte[NumberOfBytes]);
			var result = sequenceBytesLongEnough.Take(NumberOfBytes).Reverse();
			return result;
		}

		private IEnumerable<byte> GetGuidBytes()
		{
			var result = Guid.NewGuid().ToByteArray().Take(10).ToArray();
			return result;
		}
	}
}