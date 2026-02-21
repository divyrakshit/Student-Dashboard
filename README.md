# Student Dashboard — Async/Await Implementation

A production-grade student dashboard demonstrating real-world async/await patterns in JavaScript, built with vanilla HTML/CSS/JS.

---

## Overview

This project showcases how to use `async/await` to manage asynchronous data fetching in a student dashboard UI — covering course progress, grades, assignments, and notifications — all loaded concurrently and gracefully.

---

## Features

- **Async data fetching** with `async/await` and `Promise.all` for parallel requests
- **Loading states** per widget while data resolves
- **Error handling** with `try/catch` blocks and user-friendly fallback UI
- **Simulated API layer** (easily swappable with real endpoints)
- **Dark-themed dashboard UI** with animated status indicators

---

## Project Structure

```
student-dashboard/
├── index.html          # Main dashboard UI
├── README.md           # This file
└── js/
    ├── api.js          # Simulated async API functions
    ├── dashboard.js    # Dashboard controller (async/await orchestration)
    └── ui.js           # DOM rendering helpers
```

---

## Async/Await Patterns Used

### 1. Parallel Data Fetching
```js
async function loadDashboard() {
  const [courses, grades, assignments, notifications] = await Promise.all([
    fetchCourses(),
    fetchGrades(),
    fetchAssignments(),
    fetchNotifications()
  ]);
  renderAll({ courses, grades, assignments, notifications });
}
```

### 2. Sequential Fetching (when order matters)
```js
async function loadCourseDetails(courseId) {
  const course = await fetchCourse(courseId);
  const instructor = await fetchInstructor(course.instructorId); // depends on course
  render(course, instructor);
}
```

### 3. Error Handling Per Widget
```js
async function loadGrades() {
  try {
    const grades = await fetchGrades();
    renderGrades(grades);
  } catch (err) {
    renderError('grades', err.message);
  }
}
```

### 4. Retry Logic
```js
async function fetchWithRetry(fn, retries = 3) {
  for (let i = 0; i < retries; i++) {
    try {
      return await fn();
    } catch (err) {
      if (i === retries - 1) throw err;
      await delay(1000 * (i + 1)); // exponential backoff
    }
  }
}
```

### 5. Loading State Management
```js
async function withLoading(widgetId, asyncFn) {
  showSkeleton(widgetId);
  try {
    const data = await asyncFn();
    hideSkeletion(widgetId);
    return data;
  } catch (err) {
    showError(widgetId, err);
  }
}
```

---

## Simulated API Layer

All API functions return `Promise`s with artificial delays to simulate network latency:

```js
function fetchCourses() {
  return new Promise((resolve) => {
    setTimeout(() => resolve(MOCK_COURSES), 800);
  });
}
```

To connect to a real backend, replace each function body with a `fetch()` call:

```js
async function fetchCourses() {
  const res = await fetch('/api/courses');
  if (!res.ok) throw new Error('Failed to load courses');
  return res.json();
}
```

---

## Getting Started

1. Clone or download the project
2. Open `index.html` in any modern browser — no build step required
3. To connect real APIs, update the functions in `js/api.js`

---

## Browser Support

Works in all modern browsers that support ES2017+ (`async/await`):
- Chrome 55+
- Firefox 52+
- Safari 10.1+
- Edge 15+

For older browser support, transpile with **Babel** and bundle with **Vite** or **Webpack**.

---

## Key Concepts Reference

| Concept | Description |
|---|---|
| `async function` | Declares a function that returns a Promise |
| `await` | Pauses execution until a Promise resolves |
| `Promise.all()` | Runs multiple async operations in parallel |
| `Promise.allSettled()` | Like `Promise.all()` but doesn't fail fast |
| `try/catch` | Handles rejected Promises gracefully |
| `Promise.race()` | Resolves/rejects with the first settled Promise |

---

## License

MIT — free to use and modify for educational and commercial projects.
