<Query Kind="Program" />

record Color(string Name, string Category, string Foreground, string Background, XElement Element);

Color[] GetColors(string path)
{
	var xdoc = XDocument.Load(path);

	return xdoc.Root.Element("Theme")
		.Descendants().Where(n => n.Name.LocalName == "Color")
		.Select(color => new Color(
			color.Attribute("Name").Value,
			color.Parent.Attribute("Name").Value,
			color.Element("Foreground")?.Attribute("Source")?.Value,
			color.Element("Background")?.Attribute("Source")?.Value,
			color))
		.ToArray();
}

void Main()
{
	var sourcePath = @"C:\dev\JFlepp.VS.Theme.DarkYellow\JFlepp.VS.Theme.DarkYellow\CustomTheme.vstheme";
	var targetPath = @"C:\dev\JFlepp.VS.Theme.DarkYellow\JFlepp.VS22.Theme.DarkYellow\CustomTheme.vstheme";

	var sourceColors = GetColors(sourcePath).ToArray();
	var targetColors = GetColors(targetPath).ToArray();

	var targetDoc = XDocument.Load(targetPath).Root;
	foreach (var sourceColor in sourceColors)
	{
		var targetNode = targetDoc.Descendants()
			.Where(n => n.Name.LocalName == "Category" && n.Attribute("Name").Value == sourceColor.Category)
			.SelectMany(category => category.Elements())
			.SingleOrDefault(n => n.Attribute("Name")?.Value == sourceColor.Name);
		
		if (targetNode is null) continue;
		
		targetNode.ReplaceWith(sourceColor.Element);
	}
	File.WriteAllText(targetPath, targetDoc.ToString());	
}

