using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;




public class IronSourceSegment
{

	public int age;
	public string gender = null;
	public int level;
	public int isPaying;
	public long userCreationDate;
	public double iapt;
	public string segmentName = null;
	public Dictionary<string,string> customs;

	public IronSourceSegment ()
	{
		customs = new Dictionary<string,string> ();
		age = -1;
		level = -1;
		isPaying = -1;
		userCreationDate = -1;
		iapt = 0;
	}

	public void setCustom(string key, string value){
		customs.Add (key, value);
	}

	public Dictionary<string,string> getSegmentAsDict ()
	{
		Dictionary<string,string> temp = new Dictionary<string,string> ();
		if (age != -1)
			temp.Add ("age", age + "");
		if (!string.IsNullOrEmpty(gender))
			temp.Add ("gender", gender);
		if (level != -1)
			temp.Add ("level", level + "");
		if (isPaying > -1 && isPaying < 2)
			temp.Add ("isPaying", isPaying + "");
		if (userCreationDate != -1)
			temp.Add ("userCreationDate", userCreationDate + "");
		if (!string.IsNullOrEmpty(segmentName))
			temp.Add ("segmentName", segmentName);
		if (iapt > 0)
			temp.Add ("iapt", iapt + "");

		Dictionary<string,string> result = temp.Concat (customs).GroupBy (d => d.Key).ToDictionary (d => d.Key, d => d.First ().Value);
		
		return result;
		
	}
		





}


