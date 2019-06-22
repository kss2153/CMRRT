using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System.Linq;

public class Data : MonoBehaviour
{

	public  List <Ingredient> ingredients = new List<Ingredient> ();
	public  List <Experiment> records = new List<Experiment> ();

	public Dictionary<HashSet<Ingredient>, List<Experiment>> dict = new Dictionary<HashSet<Ingredient>, List<Experiment>> (new IngredientArrEqualityComparer());
	public List<Mix> mixes = new List<Mix> ();


	void Awake ()
	{
		fill_data ();
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public class IngredientArrEqualityComparer : IEqualityComparer<HashSet<Ingredient>>
	{
		public bool Equals (HashSet<Ingredient> x, HashSet<Ingredient> y)
		{
			if (x.Count != y.Count) {
				return false;
			}
			HashSet<string> x_names = new HashSet<string> ();
			foreach (Ingredient i in x) {
				x_names.Add (i.name);
			}
			foreach (Ingredient j in y) {
				if (!x_names.Contains (j.name)) {
					return false;
				}
			}
			return true;
		}

		public int GetHashCode (HashSet<Ingredient> obj)
		{
			int result = 17;
			foreach (Ingredient i in obj) {
				unchecked {
					result = result + i.name.GetHashCode();
				}
			}
			return result;
		}
	}

	public IEnumerable<IEnumerable<T>> GetPowerSet<T> (List<T> list)
	{
		return from m in Enumerable.Range (0, 1 << list.Count)
		       select
		           from i in Enumerable.Range (0, list.Count)
		 where (m & (1 << i)) != 0
		 select list [i];
	}


	void fill_data ()
	{
		Ingredient beeswax = new Ingredient ("beeswax", "Beeswax is a natural wax produced by honey bees of the genus Apis.", new HSB (51, 12, 96), "https://stakich.com/collections/beeswax/products/white-beeswax-pellets", false);
		Ingredient sulfur = new Ingredient ("sulfur", "Sulfur is a chemical element with symbol S and atomic number 16. It is abundant, multivalent, and nonmetallic.", new HSB (53, 47, 85), "https://upload.wikimedia.org/wikipedia/commons/4/44/Sulfur-sample.jpg", false);
		Ingredient rosin = new Ingredient ("rosin", "Rosin, also called colophony or Greek pitch, is a solid form of resin obtained from pines and some other plants, mostly conifers, produced by heating fresh liquid resin to vaporize the volatile liquid terpene components.", new HSB (51, 12, 96), "https://www.leafly.com/news/cannabis-101/what-is-rosin", false);
		Ingredient vermillion = new Ingredient ("vermillion", "Vermilion is both a brilliant red or scarlet pigment originally made from the powdered mineral cinnabar and the name of the resulting color.", new HSB (1, 81, 99), "https://upload.wikimedia.org/wikipedia/commons/b/ba/Red_tikka_powder.jpg", false);
		Ingredient azurite = new Ingredient ("azurite", "Azurite is a soft, deep blue copper mineral produced by weathering of copper ore deposits. It is also known as Chessylite after the type locality at Chessy-les-Mines near Lyon, France.", new HSB (226, 98, 82), "https://www.jewelsforme.com/images/articles/azurite-pigment-history-meaning.jpg", false);
		Ingredient lead = new Ingredient ("lead", "Lead is a chemical element with symbol Pb and atomic number 82.", new HSB (210, 4, 20), "http://www.thesleuthjournal.com/wp-content/uploads/2017/09/lead.jpg", false);

		Ingredient knife = new Ingredient ("knife", "tool", new HSB (210, 4, 20), "", true);
		Ingredient heat_tool = new Ingredient ("heat tool", "tool", new HSB (1, 81, 99), "", true);
		Ingredient mortar_and_pestle = new Ingredient ("mortar & pestle", "tool", new HSB (210, 4, 20), "", true);
	
		ingredients.Add (beeswax);
		ingredients.Add (sulfur);
		ingredients.Add (rosin);
		ingredients.Add (vermillion);
		ingredients.Add (azurite);
		ingredients.Add (lead);

		ingredients.Add (knife);
		ingredients.Add (mortar_and_pestle);
		ingredients.Add (heat_tool);

		foreach (IEnumerable<Ingredient> i_list in GetPowerSet (ingredients).ToArray ()) {
			dict.Add (new HashSet<Ingredient>(i_list.ToList ()), new List<Experiment> ());
		}


		Experiment MK0005 = new Experiment ("MK0005", "FA2014", "Molded Object", "J. Visco, E. Cataldo", new Ingredient[] {
			sulfur,
			beeswax
		}, new HSB (52, 29, 90), "Breadmolding", "https://www.flickr.com/photos/128418753@N06/albums/72157646455049463");
		Experiment MK0143 = new Experiment ("MK0143", "SP2015", "Imitation coral", "Project staff", new Ingredient[] {
			rosin,
			vermillion
		}, new HSB (21, 88, 84), "Pigment making", "https://www.flickr.com/photos/128418753@N06/sets/72157649281860857");
		Experiment MK0147 = new Experiment ("MK0147", "SP2016", "Dragonsblood Stains", "Robin Reich", new Ingredient[] {
			lead,
			sulfur,
			vermillion
		}, new HSB (1, 81, 99), "Colormaking", "https://www.flickr.com/photos/128418753@N06/albums/72157693449160841");
		Experiment MK0275 = new Experiment ("MK0275", "FA2017", "Painting", "CMG", new Ingredient[] { rosin, sulfur }, new HSB (47, 70, 77), "Azurite in walnut oil and egg white on panel", "https://www.flickr.com/photos/128418753@N06/albums/72157662808540918");
		Experiment MK0184 = new Experiment ("MK0184", "FA2016", "flowers in vinegar", "Caitlyn Sellar", new Ingredient[] {
			azurite,
			lead
		}, new HSB (226, 80, 51), "Keeping Dried Flowers", "https://www.flickr.com/photos/128418753@N06/albums/72157676660204990");
		Experiment MK0276 = new Experiment ("MK0276", "FA2017", "Tests for fol. 42v and 10v", "Sumeyye Yar", new Ingredient[] {
			rosin,
			azurite,
			vermillion
		}, new HSB (7, 41, 53), "Annotation", "https://www.flickr.com/photos/128418753@N06/sets/72157670475565245");
		Experiment MK0051 = new Experiment ("MK0051", "FA2014", "Molded Objects, medals ", "M. Lee, Y. Wang, D. Mellon", new Ingredient[] {
			beeswax,
			azurite
		}, new HSB (225, 41, 83), "Metalmolding", "https://www.flickr.com/photos/128418753@N06/albums/72157670381813026");
		Experiment MK0151 = new Experiment ("MK0151", "SP2016", "Sulfuric Acid and PArchment Project", "Olivia Clemens", new Ingredient[] {
			vermillion,
			lead
		}, new HSB (1, 67, 59), "Color making", "https://www.flickr.com/photos/128418753@N06/sets/72157667381374256");
		Experiment MK0155 = new Experiment ("MK0155", "SP2016", "gilden paper and parchment", "Goldenberg", new Ingredient[] {
			beeswax,
			rosin,
			azurite,
			sulfur
		}, new HSB (20, 52, 91), "Gilded paper and parchment", "https://www.flickr.com/photos/128418753@N06/sets/72157667391388441");
		Experiment MK0109 = new Experiment ("MK0109", "SP2015", "Molded object, rose", "G. Storeck", new Ingredient[] {
			sulfur,
			beeswax
		}, new HSB (335, 61, 51), "Metalmolding", "https://www.flickr.com/photos/128418753@N06/sets/72157670475889525");
		Experiment MK0017 = new Experiment ("MK0017", "FA2014", "Molded Object", "J. Boulboulle", new Ingredient[] {
			beeswax,
			rosin
		}, new HSB (45, 49, 82), "Breadmolding", "https://www.flickr.com/photos/128418753@N06/sets/72157669451216660");
			


// ~~~ add fake data with tools


		Experiment MK9005 = new Experiment ("MK9005", "FA2014", "Molded Object + Mortar and Pestle", "J. Visco, E. Cataldo", new Ingredient[] {
			sulfur,
			beeswax,
			mortar_and_pestle
		}, new HSB (52, 29, 90), "Breadmolding", "https://www.flickr.com/photos/128418753@N06/albums/72157646455049463");
		Experiment MK9995 = new Experiment ("MK9995", "FA2014", "fake data", "J. Visco, E. Cataldo", new Ingredient[] {
			sulfur,
			beeswax
		}, new HSB (52, 29, 90), "Breadmolding + knife", "https://www.flickr.com/photos/128418753@N06/albums/72157646455049463");
		Experiment MK9143 = new Experiment ("MK9143", "SP2015", "Imitation coral + knife", "Project staff", new Ingredient[] {
			rosin,
			vermillion,
			knife
		}, new HSB (21, 88, 84), "Pigment making", "https://www.flickr.com/photos/128418753@N06/sets/72157649281860857");
		Experiment MK9147 = new Experiment ("MK9147", "SP2016", "Dragonsblood Stains + heat", "Robin Reich", new Ingredient[] {
			lead,
			sulfur,
			vermillion,
			heat_tool
		}, new HSB (1, 81, 99), "Colormaking", "https://www.flickr.com/photos/128418753@N06/albums/72157693449160841");
		Experiment MK9275 = new Experiment ("MK9275", "FA2017", "Painting + knife", "CMG", new Ingredient[] { 
			rosin, 
			sulfur,
			knife
		}, new HSB (47, 70, 77), "Azurite in walnut oil and egg white on panel", "https://www.flickr.com/photos/128418753@N06/albums/72157662808540918");
		Experiment MK9184 = new Experiment ("MK9184", "FA2016", "flowers in vinegar + knife", "Caitlyn Sellar", new Ingredient[] {
			azurite,
			lead,
			knife
		}, new HSB (226, 80, 51), "Keeping Dried Flowers", "https://www.flickr.com/photos/128418753@N06/albums/72157676660204990");
		Experiment MK9276 = new Experiment ("MK9276", "FA2017", "Tests for fol. 42v and 10v + Mortar and Pestle", "Sumeyye Yar", new Ingredient[] {
			rosin,
			azurite,
			vermillion,
			mortar_and_pestle
		}, new HSB (7, 41, 53), "Annotation", "https://www.flickr.com/photos/128418753@N06/sets/72157670475565245");
		Experiment MK9051 = new Experiment ("MK9051", "FA2014", "Molded Objects, medals + Heat", "M. Lee, Y. Wang, D. Mellon", new Ingredient[] {
			beeswax,
			azurite,
			heat_tool
		}, new HSB (225, 41, 83), "Metalmolding", "https://www.flickr.com/photos/128418753@N06/albums/72157670381813026");
		Experiment MK9151 = new Experiment ("MK9151", "SP2016", "Sulfuric Acid and PArchment Project + knife", "Olivia Clemens", new Ingredient[] {
			vermillion,
			lead,
			knife
		}, new HSB (1, 67, 59), "Color making", "https://www.flickr.com/photos/128418753@N06/sets/72157667381374256");
		Experiment MK9155 = new Experiment ("MK9155", "SP2016", "gilden paper and parchment + Heat + Mortar", "Goldenberg", new Ingredient[] {
			beeswax,
			rosin,
			azurite,
			sulfur,
			mortar_and_pestle,
			heat_tool
		}, new HSB (20, 52, 91), "Gilded paper and parchment", "https://www.flickr.com/photos/128418753@N06/sets/72157667391388441");
		Experiment MK9109 = new Experiment ("MK9109", "SP2015", "Molded object, rose + Mortar", "G. Storeck", new Ingredient[] {
			sulfur,
			beeswax,
			mortar_and_pestle
		}, new HSB (335, 61, 51), "Metalmolding", "https://www.flickr.com/photos/128418753@N06/sets/72157670475889525");
		Experiment MK9017 = new Experiment ("MK9017", "FA2014", "Molded Object + knife", "J. Boulboulle", new Ingredient[] {
			beeswax,
			rosin, knife
		}, new HSB (45, 49, 82), "Breadmolding", "https://www.flickr.com/photos/128418753@N06/sets/72157669451216660");


		records.Add (MK0005);
		records.Add (MK0143);
		records.Add (MK0147);
		records.Add (MK0275);
		records.Add (MK0184);
		records.Add (MK0276);
		records.Add (MK0051);
		records.Add (MK0151);
		records.Add (MK0155);
		records.Add (MK0109);
		records.Add (MK0017);
		addTestData ();
		records.Add (MK9005);
		records.Add (MK9995);
		records.Add (MK9143);
		records.Add (MK9147);
		records.Add (MK9275);
		records.Add (MK9184);
		records.Add (MK9276);
		records.Add (MK9051);
		records.Add (MK9151);
		records.Add (MK9155);
		records.Add (MK9109);
		records.Add (MK9017);

		foreach (Experiment e in records) {
			dict [new HashSet<Ingredient> (e.materials)].Add (e);
		}

		print (dict[new HashSet<Ingredient>() {beeswax, rosin}].Count);

		addEdges ();
	}

	void addTestData ()
	{
		foreach (Ingredient i in ingredients) {
			records.Add (new Experiment (i.name, "SP2018", i.name, "kayvon", new Ingredient[]{ i }, i.color, "Experimenting", ""));
			foreach (Ingredient j in ingredients) {
				if (j.name != i.name) {
					records.Add (new Experiment (i.name, "SP2018", i.name + " and " + j.name, "kayvon", new Ingredient[]{ i, j }, i.color, "Experimenting", ""));
				}
			}
		}
	}

	void addEdges ()
	{
		foreach (Experiment e in records) {
			foreach (Experiment e_add in records) {
				if (e_add.experiment_id == e.experiment_id)
					continue;
				bool contained = true;
				foreach (Ingredient i in e.materials) {
					if (!e_add.contains (i))
						contained = false;
				}
				if (contained) {
					if (e.materials.Length == e_add.materials.Length)
						e.repeats.Add (e_add);
					else {
						e.green_edges.Add (e_add);
						e_add.red_edges.Add (e);
					}
				}
			}
		}
	}

	// finds an ingredient based on the name
	public Ingredient findIngredient (string name)
	{
//		print ("finding " + name);
		foreach (Ingredient i in ingredients) {
//			print("checking " + i.name);
			if (i.name.Equals (name)) {
//				print ("found " + name);
				return i;
			}
		}
//		print ("could not find " + name);
		return null;
	}

	// return an array containing the names all known ingredients
	public string[] getIngredientNames ()
	{
		List<string> output = new List<string> ();
		foreach (Ingredient i in ingredients) {
			output.Add (i.name);
		}
		return output.ToArray ();
	}
}
