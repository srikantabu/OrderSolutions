// -----------------------------------------------------------------------------
// File: ErrorBox.tsx
// Project: OrderSolutions - React Client
// Description: Re-usable component for displaying API or UI errors with retry options.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

interface Props {
  message: string;
  onRetry: () => void;
}

const ErrorBox = ({ message, onRetry }: Props) => {
  return (
    <div className="my-6 flex flex-col items-center justify-center p-6 border border-red-300 bg-red-50 text-red-700 rounded">
      <p className="font-medium">{message}</p>
      <button
        onClick={onRetry}
        className="mt-3 px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
      >
        Retry
      </button>
    </div>
  );
};

export default ErrorBox;
