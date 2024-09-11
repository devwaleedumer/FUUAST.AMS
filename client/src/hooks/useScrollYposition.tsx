import { useState, useEffect } from 'react';

const useScrollPosition = () => {
  const [scrollYPosition, setScrollYPosition] = useState(0);

  useEffect(() => {
    const handleScroll = () => {
      const newScrollYPosition = window.pageYOffset;
      setScrollYPosition(newScrollYPosition);
    };

    window.addEventListener('scroll', handleScroll);

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  return scrollYPosition;
};

export default useScrollPosition;
