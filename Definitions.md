# Project defined definitions

## Workers

- Each android worker is identified by a letter of the alphabet (A, B, etc).

- Workers' activities have a start and end time.

- Build a component always performed by one worker.

- Build  a machine: performed by one or several workers togheter.

- workers can do activities during full day, all days of the year.

-  each time a worker finishes an activity needs to rest
    - 2 hours after a component.
    - 4 hours after a machine.

## BDD
### Me as the application user:
- Want to schedule activities in any time (future or past).

- When scheduling an activity, want to indicate the start and end times.

- Want to indicate the type of activity and the worker or list of workers.

- Want to be able to delete activities.

### The Application Should:
- indicate when a worker's activity conflicts with other activities of the same worker because their times overlap.

- Indicate when an activity is scheduled between the rest time of the workers.

- Show a list of the top 10 androids that are working more time in the next 7 days.
