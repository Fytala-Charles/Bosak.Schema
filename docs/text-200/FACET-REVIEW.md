# Text facet source review

The adopted XSD1.1 Datatypes body and immutable digest remain those in ADOPTION.json. Before refining inherited length combinations, §4.3.1.4 is applied explicitly: length and minLength/maxLength can coexist when the same bound exists in a restriction ancestor without length. A blanket rejection of all such combinations would misclassify valid schemas. Effective inherited facet values and the qualifying ancestor must be retained separately from effective accepted lengths. Direct primitive restrictions combining the facets without that ancestor remain Invalid. Earlier binary/list bounded implementations are not silently changed by this text-specific refinement.

Whitespace strengthening over inherited lexical constraints and enumeration over patterned ancestors remain Unsupported pending separate qualification. These are profile exclusions, not schema invalidity claims.
