using System.Collections;
using System.Collections.Generic;
using System;

namespace AssemblyCSharp
{
	
	public class Ingredient
	{
		public string name;
		public string description;
		public HSB color;
		public string image;
		public bool tool;

		public Ingredient (string n, string d, HSB c, string i, bool tool)
		{
			this.name = n;
			this.description = d;
			this.color = c;
			this.image =  i;
			this.tool = tool;
		}
	}

	public class Mix
	{
		public List<Ingredient> ingredients;
		public List<Experiment> records = new List<Experiment> ();

		public Mix(List<Ingredient> mix) {
			this.ingredients = mix;
		}

		public Mix(List<Ingredient> mix, List<Experiment> records) {
			this.ingredients = mix;
			this.records = records;
		}
	}

	public class Experiment
	{
		public string experiment_id;
		public string semester;
		public string title;
		public string description;
		public string researcher;
		public Ingredient[] materials;
		public HSB color;
		public string activity;
		public string image;
		public List<Experiment> green_edges = new List<Experiment> ();
		public List<Experiment> red_edges = new List<Experiment> ();
		public List<Experiment> repeats = new List<Experiment> ();


		string[] experimentActivities = {"Breadmolding", "Pigmentmaking", "Colormaking", "Metalmolding", "Paintmaking", "Gilding"};

		// Adding title
		public Experiment (string e, string s, string t, string d, string r, Ingredient[] m, HSB c, string a, string i)
		{
			this.experiment_id = e;
			this.semester = s;
			this.title = t;
			this.description = d;
			this.researcher = r;
			this.materials = m;
			this.color = c;				
			this.activity = a;
			this.image = i;
		}

		bool activityExists(string activity) {
			foreach (string activityType in experimentActivities) {
				if (activityType.Equals (activity)) {
					return true;
				}
			}
			return false;
		}

		// Deprecated, no real title/description
		public Experiment (string e, string s, string d, string r, Ingredient[] m, HSB c, string a, string i)
		{
			this.experiment_id = e;
			this.semester = s;
			this.description = d;
			this.researcher = r;
			this.materials = m;
			this.color = c;
			this.activity = a;
			this.image = i;
		}

		public bool contains(Ingredient other_material) {
			bool result = false;
			foreach (Ingredient i in this.materials) {
				if (i.name == other_material.name)
					result = true;
			}
			return result;
		}
	}

	public class HSB
	{
		public float h;
		public float s;
		public float b;

		public HSB(int hue, int sat, int bri)
		{
			this.h = (float)hue;
			this.s = (float)sat;
			this.b = (float)bri;
		}

		public HSB(float hue, float sat, float bri)
		{
			this.h = (float)hue;
			this.s = (float)sat;
			this.b = (float)bri;
		}
	}
}

