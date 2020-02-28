using System;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Void
{
	class ConsoleProgressBar : IConsoleProgressBar
	{
		private static readonly TimeSpan DEFAULT_SPINNER_INTERVAL = TimeSpan.FromMilliseconds(250);
		private static readonly string DEFAULT_DIGIT_FORMAT = "N0";
		private static readonly string SPINNER_STATES = @"|/-\";


		private readonly object locker;
		private readonly Timer timer;
		private string digitFormat;
		private string textValue;
		private double progress;
		private bool spinnerClockwise;
		private int spinnerCounter;
		private bool disposed;


		public int Size { get; }

		public double Progress {
			get {
				lock (this.locker) {
					return this.progress;
				}
			}
			set {
				lock (this.locker) {
					this.progress = Math.Max(0, Math.Min(1, value));
					Animate();
				}
			}
		}

		public bool IsSpinnerEnabled {
			get {
				lock (this.locker) {
					return !this.disposed && this.timer.Enabled;
				}
			}
			set {
				lock (this.locker) {
					if (!this.disposed) {
						this.timer.Enabled = value;
					}
				}
			}
		}

		public bool IsSpinnerClockwise {
			get {
				lock (this.locker) {
					return this.spinnerClockwise;
				}
			}
			set {
				lock (this.locker) {
					this.spinnerClockwise = value;
				}
			}
		}

		public TimeSpan SpinnerInterval {
			get { 
				lock (this.locker) {
					return TimeSpan.FromMilliseconds(this.timer.Interval);
				}
			}
			set { 
				lock (this.locker) {
					this.timer.Interval = value.TotalMilliseconds;
				}
			}
		}

		public string DigitFormat { 
			get {
				lock (this.locker) {
					return this.digitFormat;
				}
			}
			set {
				lock (this.locker) {
					this.digitFormat = value;
				}
			}
		}



		public ConsoleProgressBar(int size) {
			if (size < 1) {
				throw new ArgumentException(
					"Size must be greater or equal than 1", 
					nameof(size)
					);
			}
			this.Size = size;
			this.textValue = string.Empty;
			this.digitFormat = DEFAULT_DIGIT_FORMAT;
			this.timer = new Timer(DEFAULT_SPINNER_INTERVAL.TotalMilliseconds);
			this.timer.Elapsed += Animate;
			this.locker = new object();
		}


		public void Dispose() {
			lock (this.locker) {
				if (!this.disposed) {
					this.disposed = true;
					this.timer.Dispose();
					this.timer.Elapsed -= Animate;
				}
			}
		}

		public void Report(double value) {
			this.Progress = value;
		}

		private void Animate(object sender, ElapsedEventArgs e) {
			Animate();
		}

		private void Animate() {
			lock (this.locker) {
				if (!this.disposed) {
					unchecked {
						this.spinnerCounter++;
					};
					var blockCount = (int)(this.progress * this.Size);
					var progressPercent = this.progress * 100D;
					var text = new StringBuilder();
					text.Append('[');
					text.Append(new string('#', blockCount));
					text.Append(new string('-', this.Size - blockCount));
					text.Append("] ");
					text.Append(progressPercent.ToString(this.DigitFormat));
					text.Append('%');
					if (this.IsSpinnerEnabled) {
						text.Append(' ');
						var reverse = this.IsSpinnerClockwise ? 0 : SPINNER_STATES.Length;
						var index = Math.Abs(reverse - this.spinnerCounter % SPINNER_STATES.Length);
						text.Append(SPINNER_STATES[index % SPINNER_STATES.Length]);
					}
					Refresh(text.ToString());
				}
			}
		}

		private void Refresh(string text) {
			var commonLength = 0;
			var minLength = Math.Min(this.textValue.Length, text.Length);
			while (commonLength < minLength && text[commonLength] == this.textValue[commonLength]) {
				commonLength++;
			}
			var overlap = new StringBuilder();
			if (this.textValue.Length > 0) {
				overlap.Append('\b', this.textValue.Length - commonLength);
			}
			overlap.Append(text.Substring(commonLength));
			var overlapCount = this.textValue.Length - text.Length;
			if (overlapCount > 0) {
				overlap.Append(' ', overlapCount);
				overlap.Append('\b', overlapCount);
			}
			this.textValue = text;
			Console.Write(overlap);
		}
	}
}
