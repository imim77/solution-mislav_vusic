import { beforeEach, afterEach, it, expect, describe, vi } from 'vitest'
import { renderHook, act } from "@testing-library/react";
import {useDebounce} from '../src/hooks/useDebounce';


describe("useDebounce", () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });
  afterEach(() => {
    vi.useRealTimers();
  });

  it("updates the value after the delay", () => {
    const { result, rerender } = renderHook(
      ({ value }) => useDebounce(value, 500),
      {
        initialProps: { value: "hello" },
      }
    );

    expect(result.current).toBe("hello");

    rerender({ value: "world" });

    // još nije prošlo 500 ms
    expect(result.current).toBe("hello");

    act(() => {
      vi.advanceTimersByTime(500);
    });

    expect(result.current).toBe("world");
  });

})
