<Query Kind="Program" />

record Color(string Name, string Foreground, string Background, XElement element);

Color[] GetColors(string path)
{
	var xdoc = XDocument.Load(path);

	return xdoc.Root.Element("Theme")
		.Elements().SelectMany(category => category.Elements())
		.Select(color => new Color(
			color.Parent.Attribute("Name").Value + " -> " + color.Attribute("Name").Value,
			color.Element("Foreground")?.Attribute("Source")?.Value,
			color.Element("Background")?.Attribute("Source")?.Value,
			color))
		.ToArray();
}

void Main()
{
	var jfleppThemePath = @"C:\dev\JFlepp.VS.Theme.DarkYellow\JFlepp.VS.Theme.DarkYellow\CustomTheme.vstheme";
	var defaultThemePath = @"C:\temp\DefaultVSTheme\CustomTheme.vstheme";

	var jfleppColors = GetColors(jfleppThemePath).ToDictionary(c => c.Name, c => c);
	var darkColors = GetColors(defaultThemePath).ToDictionary(c => c.Name, c => c);

	var additionalColors = darkColors.Keys.Where(n => !jfleppColors.Keys.Contains(n)).ToArray();
	
	var jfleppXDoc = XDocument.Load(jfleppThemePath).Root;
	foreach (var additionalColor in additionalColors.Select(c => darkColors[c].element))
	{
		var parentNode = jfleppXDoc.Descendants()
			.SingleOrDefault(n => n.Attribute("Name")?.Value == additionalColor.Parent.Attribute("Name").Value);
		if (parentNode == null)
		{
			jfleppXDoc.Element("Theme").Add(additionalColor.Parent);
			continue;
		}
		if (!parentNode.Elements().Any(e => e.Attribute("Name").Value == additionalColor.Attribute("Name").Value))
		{
			parentNode.Add(additionalColor);
		}
	}
	additionalColors.Select(c => darkColors[c]).SelectMany(c => new[] { c.Foreground, c.Background }).Distinct().Dump();
	File.WriteAllText(jfleppThemePath, jfleppXDoc.ToString());	
}

