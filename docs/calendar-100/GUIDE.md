# Exact native calendars

The development candidate admits `xs:dateTime`, `xs:time` and `xs:dateTimeStamp` through both native scalar facades. It requires signed Bosak Core 1.86.0. Published Schema beta.1 keeps its historical scope; this guide describes the new source candidate.

```csharp
using Bosak.Schema;
using Bosak.XPath.Core.Xdm;

var schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:dateTimeStamp'/></xs:schema>";
var options = new NativeSchemaSessionOptions(64, 32, 256, 65536, 1, 32, 1, 64, 32);
using var session = NativeSchemaSession.Create(schema, options).Value!;
using var document = session.Assess(
    "<r>2026-11-25T12:00:00.123456789123+01:00</r>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0)).Value!;
var value = document.ReadRootScalar().CalendarValue;
Console.WriteLine(value.Second.ToInvariantString(64));
var utc = XdmExactCalendarValue.Parse("2026-11-25T11:00:00.123456789123Z",
    XdmExactCalendarKind.DateTimeStamp, 64, 32);
Console.WriteLine(XdmExactCalendarComparison.CompareXsd(value, utc));
```

Expected output is `00.123456789123` followed by `Equal`. In an application, check each operation's outcome before reading `Value`; Invalid and Unsupported return no document.

`NativeScalarKind.DateTime`, `Time` and `DateTimeStamp` have values 7, 8 and 9. Existing values 1 through 6 remain unchanged. `NativeScalarValue.CalendarValue` returns an immutable `XdmExactCalendarValue`; use `Kind`, calendar components, `Second.Coefficient`, `Second.Scale`, `HasTimezone` and `TimezoneOffsetMinutes`. A detached value remains usable after document/session disposal and carries no schema authority. Other scalar accessors reject calendars, and `CalendarValue` rejects noncalendar values.

The original-node facade uses `ReadElement(path)` or `ReadAttribute(ownerPath, namespaceUri, localName)`. Scalar sessions retain `ReadRootScalar`, `ReadScalar`, `ReadAttribute` and `ReadDescendantAttribute`. Existing scalar/path indexes and original XML export remain intact. Named/anonymous restrictions, existing finite sequence/choice/all placements, attributes/references/groups, simple-content extension and bounded complex restriction retain original declaration and actual type identities.

Calendar restrictions admit enumeration, four ordering bounds, `explicitTimezone`, fixed `whiteSpace="collapse"` and the existing finite pattern grammar. Same-step patterns are alternatives; inherited pattern groups are conjunctions. Patterns match the complete collapsed instance literal; the pattern expression itself retains its whitespace. Inherited bounds and enumerations narrow the base. Bounds over patterned ancestors remain Unsupported pending separate value-space qualification.

Enumeration and provided fixed-value validation use exact calendar value equality, retaining timezone presence. Bounds use XSD partial order, so an indeterminate comparison does not satisfy a bound. Assessment's implicit timezone never supplies a missing schema timezone. `dateTimeStamp` requires an explicit timezone and derives from `dateTime`; `time` is a separate family. UTC day displacement is retained when comparing times.

Raw calendar text is limited to 256 UTF-16 code units and `MaxStringCharacters`. `MaxNativeCoefficientDigits` counts both supplied whole-second digits plus fractional digits; `MaxNativeScale` bounds the supplied fractional digits before trailing-zero normalization. Receiving query digit/scale limits apply to the stored exact seconds. The XSD signed-zero year spelling `-0000` maps to year zero; original pattern/export text is preserved. Schema normalizes only the argument supplied to the narrower Core 1.86.0 lexical helper. Years are bounded to `[-Int64.MaxValue, Int64.MaxValue]`; 24:00 normalization beyond the upper endpoint refuses. No millisecond rounding or implicit conversion to `DateTimeOffset` occurs.

Provided fixed/default metadata follows the existing original-node contract. Missing/default application still requires a separate augmented-node design. Simple nil returns the existing typed-empty value. Cancellation, the one-document admission gate, disposal and revoked/foreign authority refusals remain enforced.

The source tests also qualify the internal native issuer against Core's closed guarded comparison profile, with exact static-context fingerprint binding and calendar-only owned derivation proof. This does not expose a new public Schema query-session factory or accept general C45/PSVI integration. Public detached comparisons can use the explicit Core APIs shown above; default XPath operators/maps are unchanged.

[Contract](CONTRACT.md), [ledger](CONTRACTS.json), [specification adoption](ADOPTION.json) and [coverage delta](COVERAGE-DELTA.json) define the precise scope. Lists/unions, durations, floating/binary/QName families, arbitrary assertions, full regex, default-node augmentation and complete XSD 1.1 conformance remain outside this batch.
