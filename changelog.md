# GeoTagNinja Changelog

**Build 8839 [20240314] ** 
- NEW & UPDATED:
  - Added changlog.md and patched readme.md a bit

- BUGS & FIXES:
  - Changed the HttpClient call process to split calls into chunks of (2x) 50s. 
	- HttpClient can be forced to close connections but it's apparently misbehaving and I'm not skilled quite enough to build a factory.
	- The original script then basically overloaded the ports with almost 23k requests and soft-killed my router. Joy. The chunks-logic has eliminated this issue.
  - Patched the code not to break on a 404 error.
  - Minor fixes.

**Build 8838 [20240313] ** 
- NEW & UPDATED:
  - Initial commit. Minor imperfections as one'd expect.

- BUGS & FIXES:
  - N/A