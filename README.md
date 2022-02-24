# DEE Impression Checker

Get the impression status of a specific ID at the venue DEE2(https://manbow.nothing.sh/event/event.cgi).

# Features

- Can check if you impressioned.
- Can exclude song from the check target. 

# Usage

1. Input "Venue URL" (ex. https://manbow.nothing.sh/event/event.cgi?action=List_def&event=138)
1. Input "Impression ID"[1].
1. Press "Get List" button.
1. If the venue URL is correct, the acquired list will be displayed. 
1. Check "Avoid" if you can't/don't impress the song.
1. Press "Start" button.
1. Check one page per second to see if it's been impressed.

[1] (VOTE) ID below name, ex. (XXXXXXXXXXXXXXXXXXJP) -> input XXXXXXXXXXXXXXXXXXJP, 
or ID written later date, ex. 2022年02月23日22:22(XXXXXXXXXXXXXXXXXXJP) -> input XXXXXXXXXXXXXXXXXXJP,
or name◆XXXXXXXXXXXX -> input XXXXXXXXXXXX,
or name(not recommended).

# License

The source code is licensed MIT.

```
AngleSharp (MIT)                             https://github.com/AngleSharp/AngleSharp
Microsoft.Data.Sqlite.Core (MIT)             https://github.com/dotnet/efcore
SQLitePCLRaw.bundle_e_sqlite3 (Apache-2.0)   https://github.com/ericsink/SQLitePCL.raw
```