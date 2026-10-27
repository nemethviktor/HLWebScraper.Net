# HLWebscraper.Net Changelog

**Next release [not yet built, no date]**
- NEW & UPDATED:
  - Auto-update: the app checks GitHub releases at startup (at most once a week) and offers to download and apply a newer version
  - The build now also produces a release zip (`HLWebScraper.Net_b[build].zip`, in the `zip` folder of the output) containing just the exe, readme, changelog and `ETF_Types_example.csv`
- BUGS & FIXES:
  - N/A

**Build 9794 [20261025]** 
- NEW & UPDATED:
  - Faster scraping: replaced the fixed-size batches with a sliding window of requests so one slow response no longer stalls the whole batch
  - Page parsing now runs off the UI thread
  - The build now also produces a single-file exe (`single` folder in the output)
  - Minor export/search performance tweaks and code tidy-up; removed an unused package
- BUGS & FIXES:
  - `Year high` was being populated with the Year low value, now fixed
  - A failed page request is now logged and skipped instead of aborting the scrape
  - About box no longer relies on `Assembly.Location` (incompatible with single-file apps)

**Build 9752 [20260913]** 
- NEW & UPDATED:
  - Updated to work with current HL Website
  - Added `Select All/None` buttons
  - Some packages updated
  - Removed obsolete/dead fields (`Country`, `Exchange`, `Indicies`)
- BUGS & FIXES:
  - N/A

**Build 9456 [20251121]** 
- NEW & UPDATED:
  - Updated packages
- BUGS & FIXES:
  - Updated logic to open the classification CSV as read-only so the app doesn't crash if it's open elsewhere

**Build 8843 [20240318]** 
- NEW & UPDATED:
  - Added a notification handler/icon to inform of process completion.
- BUGS & FIXES:
  - Reverted the no-loops-logic introduced in 8841 - it seemingly resulted in timeouts due to rate limiting on the server side whereas the loops don't. This comes at a minor performance penalty but at least doesn't ban the user.

**Build 8842 [20240317]** 
- NEW & UPDATED:
  - Against my better judgement I've moved the "Settings" to a SQLite File in the Roaming folder. This is because
	- I know SQL (and the dragon isn't sleeping, famous last words there)
	- It has worked in an another project of mine
	- I certainly won't overwrite yours accidentially when I publish an updated version of the app
  - Added a new column for Top 10 Exposures
- BUGS & FIXES:
  - I've taken the liberty to replace the "n/a" retuned values with empty string (aka nothing). It takes up less space and the original wasn't particularly useful.

**Build 8841 [20240316]** 
- NEW & UPDATED:
  - I've rewritten the HttpClient handling. Should have read up on the documentation beforehand - no more loops!
- BUGS & FIXES:
  - N/A

**Build 8840 [20240315]** 
- NEW & UPDATED:
  - Added logic to allow for limited selection of scraping targets
  - Added an ETF-only filter
  - Added an example (small) output
  - Added changlog.md and patched readme.md a bit
- BUGS & FIXES:
  - Changed the HttpClient call process to split calls into chunks of (2x) 50s. 
	- HttpClient can be forced to close connections but it's apparently misbehaving and I'm not skilled quite enough to build a factory.
	- The original script then basically overloaded the ports with almost 23k requests and soft-killed my router. Joy. The chunks-logic has eliminated this issue.
  - Patched the code not to break on a 404 error.
  - Changed the logic to exclude items from ETF classification that have > 0 Market Cap. Those are actually companies.
  - Minor fixes.

**Build 8838 [20240313] [Unreleased]** 
- NEW & UPDATED:
  - Initial commit. Minor imperfections as one'd expect.
- BUGS & FIXES:
  - N/A