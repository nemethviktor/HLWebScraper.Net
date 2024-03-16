# HLWebscraper.Net Changelog

**Build 88xx [202403xx]** 
- NEW & UPDATED:
  - I've rewritten the HttpClient handling. Should have read up on the documentation beforehand - no more loops!
  - Against my better judgement I've moved the "Settings" to a SQLite File in the Roaming folder. This is because
	- I know SQL (and the dragon isn't sleeping, famous last words there)
	- It has worked in an another project of mine
	- I certainly won't overwrite yours accidentially when I publish an updated version of the app
  
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

**Build 8838 [20240313]** 
- NEW & UPDATED:
  - Initial commit. Minor imperfections as one'd expect.

- BUGS & FIXES:
  - N/A