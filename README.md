# Graph Generator

I developed this program to randomly generate small graphs which I can then use in lectures and assignments.
The program was written in C# with Visual Studio 2019.

![Main window with a randomly generated graph (using triangulation).](mainWindow.png "Main window with a randomly generated graph (using triangulation).")

## Features

- Randomly generate small graphs with one of four approaches.
- Draw graph using a forced-based approach.
- Modify a drawing by moving vertices.
- Add and remove edges directly in the drawing.
- Generate the [TikZ](https://github.com/pgf-tikz/pgf) code for the shown graph.

## Generating a Graph

There are for ways to generate a graph:

- **Random.**
  Generates a random graph with *n* vertices and *m* edges.
  The graph is guaranteed to be connected.
  All other edges are up to chance.

- **Triangulation.**
  The program places vertices randomly on the plane and then creates a [Delaunay triangulation](https://en.wikipedia.org/wiki/Delaunay_triangulation) for these vertices.
  The generated graph is therefore planar.

- **Planar.**
  The program first places vertices randomly on the plane
  It then uses an approch from distributed sensor networks to ensure that the resulting graph is connected and planar.
  In comparison to a triangulation, the graph has notably fewer edges.

- **Layering Partition.**
  The idea is to generate a graph with an interesting layering partition.
  For that, the program first generates a small random tree *T*.
  The nodes of *T* then become the cluster of a layering partition.
  Vertices are added randomly into the clusters.
  The generated graph is not always planar.