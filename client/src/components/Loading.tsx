// -----------------------------------------------------------------------------
// File: Loading.tsx
// Project: OrderSolutions - React Client
// Description: Centralized loading spinner component.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

interface Props {
  loading: boolean;
}
const Loading = ({ loading }: Props) => {
  return (
    <>
      {loading && (
        <div className="absolute inset-0 flex items-center justify-center bg-gray-100 bg-opacity-60 z-20">
          <div className="animate-spin rounded-full h-10 w-10 border-4 border-gray-300 border-t-blue-600" />
        </div>
      )}
    </>
  );
};

export default Loading;
