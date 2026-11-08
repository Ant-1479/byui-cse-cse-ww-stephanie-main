using Microsoft.VisualStudio.TestTools.UnitTesting;

// Problem 2 - Test cases written from the requirements; defects found are recorded above each test.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue A (pri 1), B (pri 5), C (pri 3) and check the queue contents.
    // Expected Result: [A (Pri:1), B (Pri:5), C (Pri:3)] (items are added to the back regardless of priority)
    // Defect(s) Found: None. Test passed before and after the fixes (Requirement 1).
    public void TestPriorityQueue_EnqueueAddsToBack()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 5);
        priorityQueue.Enqueue("C", 3);

        Assert.AreEqual("[A (Pri:1), B (Pri:5), C (Pri:3)]", priorityQueue.ToString());
    }

    [TestMethod]
    // Scenario: Enqueue Low (1), High (5), Mid (3), then dequeue three times.
    // Expected Result: High, Mid, Low
    // Defect(s) Found: Test failed. The second Dequeue returned High again because Dequeue never
    // removed the item from the list. Fixed by calling RemoveAt on the selected index (Requirement 2).
    public void TestPriorityQueue_HighestPriorityFirst()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("High", 5);
        priorityQueue.Enqueue("Mid", 3);

        Assert.AreEqual("High", priorityQueue.Dequeue());
        Assert.AreEqual("Mid", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue A (5), B (5), C (1), then dequeue three times.
    // Expected Result: A, B, C (the earlier item wins a tie for highest priority)
    // Defect(s) Found: Test failed. B was returned before A because the comparison used >= instead of >,
    // so a later item with equal priority replaced the earlier one (Requirement 3).
    public void TestPriorityQueue_TieUsesFifo()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 5);
        priorityQueue.Enqueue("B", 5);
        priorityQueue.Enqueue("C", 1);

        Assert.AreEqual("A", priorityQueue.Dequeue());
        Assert.AreEqual("B", priorityQueue.Dequeue());
        Assert.AreEqual("C", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue A (1), B (2), C (9) so the highest priority is the last item, then dequeue once.
    // Expected Result: C
    // Defect(s) Found: Test failed. B was returned because the loop bound (Count - 1) skipped the last
    // item in the queue. Fixed by looping while index < Count (Requirement 2).
    public void TestPriorityQueue_HighestPriorityAtEnd()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 2);
        priorityQueue.Enqueue("C", 9);

        Assert.AreEqual("C", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue A (1), B (7), C (7) with the tie for highest at the end, then dequeue once.
    // Expected Result: B (the first of the tied items), and the queue then holds [A (Pri:1), C (Pri:7)]
    // Defect(s) Found: Test failed. >= selected C, and the item was never removed from the list.
    // Fixed by using > and calling RemoveAt (Requirements 2 and 3).
    public void TestPriorityQueue_TieAtEndRemovesCorrectItem()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 7);
        priorityQueue.Enqueue("C", 7);

        Assert.AreEqual("B", priorityQueue.Dequeue());
        Assert.AreEqual("[A (Pri:1), C (Pri:7)]", priorityQueue.ToString());
    }

    [TestMethod]
    // Scenario: Enqueue one item, dequeue it, then dequeue again from the now-empty queue.
    // Expected Result: First Dequeue returns the item; second throws InvalidOperationException
    // with the message "The queue is empty."
    // Defect(s) Found: Test failed. The second Dequeue returned the item again instead of throwing because
    // the item was never removed. Fixed by calling RemoveAt (Requirements 2 and 4).
    public void TestPriorityQueue_SingleItemThenEmpty()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Only", 3);
        Assert.AreEqual("Only", priorityQueue.Dequeue());

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail($"Unexpected exception of type {e.GetType()} caught: {e.Message}");
        }
    }

    [TestMethod]
    // Scenario: Dequeue from a brand-new empty queue.
    // Expected Result: InvalidOperationException with the message "The queue is empty."
    // Defect(s) Found: None. Test passed before and after the fixes (Requirement 4).
    public void TestPriorityQueue_EmptyQueue()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail($"Unexpected exception of type {e.GetType()} caught: {e.Message}");
        }
    }
}