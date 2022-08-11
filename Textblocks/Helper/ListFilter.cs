using System;
using System.Collections.Generic;
using System.Linq;

namespace cwsoft.Textblocks.Helper;

// This class provides a basic logical filter (AND/OR/NOT) for generic lists.
// Filter pattern(s) are applied to the specified input list property.
internal class ListFilter
{
   #region // Fields, Properties, Constructors
   // Fields.
   private const int _defaultPatternLength = 3;
   private int _minimumPatternLength = 0;

   // Properties.
   public string InputPattern { get; private set; } = string.Empty;
   public string ValidatedPattern { get; private set; } = string.Empty;
   public List<string> InclusivePatterns { get; private set; } = new();
   public List<string> ExclusivePatterns { get; private set; } = new();

   // Constructors.
   public ListFilter() : this(pattern: "") { }
   public ListFilter(string pattern, int minimumPatternLength = _defaultPatternLength)
      => SetPattern(pattern, minimumPatternLength);
   #endregion

   #region // Public API
   // Set new search pattern and validate it.
   public void SetPattern(string pattern, int minimumPatternLength = _defaultPatternLength)
   {
      _minimumPatternLength = Math.Max(_defaultPatternLength, minimumPatternLength);
      InputPattern = pattern?.ToLower().Trim() ?? string.Empty;
      Validate();
   }

   // Reset filter into defined state.
   public void ResetPattern()
   {
      (InputPattern, ValidatedPattern) = (string.Empty, string.Empty);
      InclusivePatterns.Clear();
      ExclusivePatterns.Clear();
   }

   // Return list of objects which object property matches the specified filter patterns.
   // Flag "operatorAnd" indicates if ALL or ANY inclusive pattern needs to match.
   public List<T> GetMatches<T>(List<T> inputList, string propertyName, bool operatorAnd = true)
   {
      if (string.IsNullOrEmpty(ValidatedPattern)) {
         return inputList;
      }

      // Apply inclusive (and/or) filter on input list.
      inputList = operatorAnd ? ApplyAndFilter(inputList, propertyName) : ApplyOrFilter(inputList, propertyName);

      // Apply exclusive filter (not) on input list.
      inputList = ApplyNotFilter(inputList, propertyName);

      return inputList;
   }
   #endregion

   #region // Internal API
   // Validate user specified filter pattern.
   private void Validate()
   {
      ValidatedPattern = InputPattern;
      if (string.IsNullOrWhiteSpace(ValidatedPattern)) {
         return;
      }

      // Split filter pattern into sub patterns and remove empty, duplicate and too short patterns.
      var patterns = ValidatedPattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct()
         .Where(x =>
            (!x.StartsWith("-") && x.Length >= _minimumPatternLength) ||
            (x.StartsWith("-") && x.Length > _minimumPatternLength))
         .ToList();

      // Store validated filter pattern.
      ValidatedPattern = string.Join(" ", patterns).Trim();

      // Split validated patterns into inclusive (AND/OR) and exlcusive (NOT) patterns.
      // Patterns starting with "-" (e.g.: "-pattern3") are exlusive, all other patterns inclusive (AND/OR).
      InclusivePatterns = patterns.Where(x => !x.StartsWith("-") && x.Length >= _minimumPatternLength).ToList();
      ExclusivePatterns = patterns.Where(x => x.StartsWith("-") && x.Length > _minimumPatternLength).Select(x => x.Substring(1)).ToList();
   }

   // Add objects where property contain ALL specified AND patterns.
   private List<T> ApplyAndFilter<T>(List<T> inputList, string propertyName)
   {
      inputList = inputList
         .Where(x => InclusivePatterns.All(y => x?.GetType().GetProperty(propertyName)?.GetValue(x)?.ToString()
         ?.IndexOf(y, StringComparison.CurrentCultureIgnoreCase) > -1))
         .ToList();
      return inputList;
   }

   // Add all objects where property contain at least ONE specified OR pattern.
   private List<T> ApplyOrFilter<T>(List<T> inputList, string propertyName)
   {
      inputList = inputList
         .Where(x => InclusivePatterns.Any(y => x?.GetType().GetProperty(propertyName)?.GetValue(x)?.ToString()
         ?.IndexOf(y, StringComparison.CurrentCultureIgnoreCase) > -1))
         .ToList();
      return inputList;
   }

   // Remove all objects where property contain at least one specified NOT pattern.
   private List<T> ApplyNotFilter<T>(List<T> inputList, string propertyName)
   {
      inputList = inputList
         .Where((x => ExclusivePatterns.All(y => x?.GetType().GetProperty(propertyName)?.GetValue(x)?.ToString()
         ?.IndexOf(y, StringComparison.CurrentCultureIgnoreCase) == -1)))
         .ToList();
      return inputList;
   }
   #endregion
}
