// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 September 2026
// PURPOSE              : Grants schema regression tests access to internal implementation details.
// SPECIAL NOTES        : Only the schema test assembly receives internal access; no commercial tool is referenced.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-09-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 05-10-2026     | Added GracePeriod, trial/Community acceptance, test clock seam (REQ-002/003/004)         |
//                      | Charles Korthout | 0.3   | 29-10-2026     | Document the legacy activation API as optional compatibility behavior                    |

//                      | Charles Korthout | 0.4   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bosak.Schema.Tests")]
