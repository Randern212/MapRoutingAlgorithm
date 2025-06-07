# 🗺️ Map Routing Algorithm - C# Console Application

A C# console application implementing a pathfinding algorithm to find the shortest path on a grid-based map. The program allows the user to define start and end points, obstacles, and then visualizes the routing steps in the console.

---

## 📂 Repository

[GitHub Repository](https://github.com/Randern212/MapRoutingAlgorithm)

---

## 🚀 Features

- Console-based grid map visualization  
- Input for start point, end point, and obstacles  
- Implementation of a shortest path algorithm (e.g., A* or Dijkstra)  
- Step-by-step visualization of the search process in console output  
- Displays the final path with clear markers  

---

## 🛠️ Technologies Used

| Technology | Description                    |
|------------|--------------------------------|
| C#         | Application logic and UI       |
| .NET       | Console application framework  |

---

## 🗺️ How the Algorithm Works

The algorithm implemented is a graph-based shortest path finder working on a 2D grid. Here’s a simplified explanation:

1. **Initialize** the open list with the start node.  
2. **Loop**:  
   - Select the node with the lowest cost estimate (distance from start + heuristic).  
   - If this node is the goal, reconstruct the path and finish.  
   - Otherwise, explore neighbors (up, down, left, right).  
   - Ignore obstacles and already visited nodes.  
   - Update costs and parents for neighbors as needed.  
   - Add neighbors to the open list for further exploration.  
3. **Repeat** until the path is found or no nodes remain.

---

## 📊 Algorithm Flow Diagram

```plaintext
[Start Node]
    ↓
[Add to Open List]
    ↓
[While Open List Not Empty]
    ↓
[Select Node with Lowest Cost]
    ↓
[If Node is Goal → Reconstruct Path]
    ↓
[Else → Explore Neighbors]
    ↓
[Update Costs & Parents]
    ↓
[Add Valid Neighbors to Open List]
    ↓
[Repeat]
