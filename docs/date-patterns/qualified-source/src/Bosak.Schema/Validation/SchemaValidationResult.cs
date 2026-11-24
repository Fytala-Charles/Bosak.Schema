// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Validation result type for the REQ-009 S2 assertion evaluation service.
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S2 slice. Findings are explicit
//                        diagnostics — a refused or erroring assertion makes the result invalid, never silently valid.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S2)                                                                    |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      | Charles Korthout | 0.3   | 17-11-2026     | Clarify compile precondition and assertion-only service result scope                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// The outcome of validating a candidate element against the recorded assertions of a
/// compiled schema (REQ-009 S2).
/// </summary>
/// <param name="IsValid"><see langword="true"/> only when every evaluated assertion held (effective boolean
/// value true), no compile diagnostics blocked the operation and no assertion was refused or raised an evaluation error.
/// A service-produced true result describes only that operation, not complete structural validation.</param>
/// <param name="Diagnostics">Validation findings: violated assertions (<see cref="SchemaDiagnosticCodes.AssertionViolated"/>),
/// refused expressions (<see cref="SchemaDiagnosticCodes.AssertionRefused"/>), and evaluation errors
/// (<see cref="SchemaDiagnosticCodes.AssertionEvaluationError"/>). Compile findings are copied unchanged when they
/// block the operation. Empty for a service-produced true result.</param>
/// <remarks>The public constructor accepts application-supplied values; this record is not authenticated
/// native assessment evidence. Detailed legacy diagnostics do not have native bounded-stage redaction guarantees.</remarks>
public sealed record SchemaValidationResult(bool IsValid, IReadOnlyList<SchemaDiagnostic> Diagnostics);
