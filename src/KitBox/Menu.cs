using System;
using System.Collections.Generic;

namespace Evaluator
{
	public class Menu
	{
		private List<Tuple<string, Func<bool>, bool>> options;
		private string title;

		public Menu() {
			this.options = new List<Tuple<string, Func<bool>, bool>>();
		}

		public Menu set_title(string title) {
			this.title = title;
			return this;
		}

		public Menu add_option(string text, Func<bool> action) {
			this.options.Add(new Tuple<string, Func<bool>, bool>(text, action, true));
			return this;
		}

		public Menu add_option(string text, Func<bool> action, bool wait) {
			this.options.Add(new Tuple<string, Func<bool>, bool>(text, action, wait));
			return this;
		}

		public bool Run() {
			while (true) {

				Console.Clear();
				Console.WriteLine("\n");
				if (this.title != null) {
					Console.WriteLine(this.title + "\n");
				}

				for (int i = 0; i < this.options.Count; i++) {
					Console.WriteLine("\t" + (i + 1) + ") " + options[i].Item1);
				}

				int choice;

				try {
					choice = Int32.Parse(Console.ReadLine()) - 1;
					if (choice >= 0 && choice < this.options.Count) {

						Console.Clear();
						Console.WriteLine("");

						if (options[choice].Item2()) {
							if (options[choice].Item3) {
								Console.WriteLine("\nHit [enter] to continue ...");
								Console.ReadLine();
							}
							break;
						} else {
							Console.WriteLine("Could not perform the operation... :'(\n");
						}
					} else {
						Console.WriteLine("Invalid choice...\n");
					}
				} catch (FormatException) {
					Console.WriteLine("Invalid choice...\n");
				}
			}
				
			return true;
		}
	}
}

