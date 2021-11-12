<Query Kind="Program" />

record Color(string Name, string Category, string Foreground, string Background, XElement Element);

(XDocument, Color[]) GetColors(string path)
{
	var xdoc = XDocument.Load(path);

	var colors = xdoc.Root.Element("Theme")
		.Descendants().Where(n => n.Name.LocalName == "Color")
		.Select(color => new Color(
			color.Attribute("Name").Value,
			color.Parent.Attribute("Name").Value,
			color.Element("Foreground")?.Attribute("Source")?.Value,
			color.Element("Background")?.Attribute("Source")?.Value,
			color))
		.ToArray();
	
	return (xdoc, colors);
}

void Main()
{
	var themePath = @"D:\src\JFlepp.VS.Theme.DarkYellow\JFlepp.VS22.Theme.DarkYellow\CustomTheme.vstheme";
	var (doc, colors) = GetColors(themePath);

	var updates = new Dictionary<string, string>()
	{
		{ "FF252526", "FF1F1F1F" },
	};
	
	foreach (var color in colors)
	{
		if (updates.Keys.Any(k => color.Foreground == k))
		{
			color.Element.Element("Foreground").Attribute("Source").Value = updates[color.Foreground];
		}
		if (updates.Keys.Any(k => color.Background == k))
		{
			color.Element.Element("Background").Attribute("Source").Value = updates[color.Background];
		}
	}
	File.WriteAllText(themePath, doc.ToString());	
}

